using Domain.Models;
using Infraestructure.Data;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Test.IntegrationTest.Repository;

/// <summary>
///     Integration tests for <see cref="TaskRepository"/> using an in-memory database.
/// </summary>
public class TaskRepositoryTests
{
    /// <summary>
    ///     Creates a new instance of <see cref="TaskRepository"/> with an in-memory database.
    /// </summary>
    /// <param name="dbName">The unique database name for isolation between tests.</param>
    /// <returns>A <see cref="TaskRepository"/> instance.</returns>
    private TaskRepository CreateRepository(string dbName)
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: dbName) // each test uses a distinct db
            .Options;

        var context = new TodoDbContext(options);
        return new TaskRepository(context);
    }

    /// <summary>
    ///     AddTaskAsync shall add a new task to the database.
    /// </summary>
    [Fact]
    public async Task AddTaskAsync_ShallAddTaskToDatabase()
    {
        // Arrange
        var repository = CreateRepository(Guid.NewGuid().ToString()); // db unique
        var task = new TaskItem
        {
            Title = "Integration test task",
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UserId = 1
        };

        // Act
        await repository.AddAsync(task);
        await repository.SaveChangesAsync();

        var result = await repository.GetByIdAsync(task.Id, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Integration test task", result.Title);
        Assert.False(result.IsCompleted);
        Assert.Equal(1, result.UserId);
    }

    /// <summary>
    ///     DeleteTaskAsync shall remove the task from the database.
    /// </summary>
    [Fact]
    public async Task DeleteTaskAsync_ShallRemoveTaskFromDatabase()
    {
        // Arrange
        var repository = CreateRepository(Guid.NewGuid().ToString());
        var task = new TaskItem { Title = "Task to delete", UserId = 1, CreatedAt = DateTime.UtcNow };
        await repository.AddAsync(task);
        await repository.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(task);
        await repository.SaveChangesAsync();

        var result = await repository.GetByIdAsync(task.Id, 1);

        // Assert
        Assert.Null(result);
    }
}