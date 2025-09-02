using Aplication.Interfaces;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

/// <summary>
///     Implementation of <see cref="ITaskRepository"/> using Entity Framework Core.
///     Provides data access methods for <see cref="TaskItem"/> entities.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly TodoDbContext _context;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskRepository"/> class.
    /// </summary>
    /// <param name="context">The database context <see cref="TodoDbContext"/>.</param>
    public TaskRepository(TodoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Retrieves all tasks for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of <see cref="TaskItem"/>.</returns>
    public async Task<IEnumerable<TaskItem>> GetAllAsync(int userId)
    {
        return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
    }

    /// <summary>
    ///     Retrieves a task by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns>The <see cref="TaskItem"/> if found; otherwise, <c>null</c>.</returns>
    public async Task<TaskItem?> GetByIdAsync(int id, int userId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    /// <summary>
    ///     Adds a new <see cref="TaskItem"/> to the database.
    /// </summary>
    /// <param name="task">The task to add.</param>
    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
    }

    /// <summary>
    ///     Updates an existing <see cref="TaskItem"/> in the database.
    /// </summary>
    /// <param name="task">The task to update.</param>
    public Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Deletes an existing <see cref="TaskItem"/> from the database.
    /// </summary>
    /// <param name="task">The task to delete.</param>
    public Task DeleteAsync(TaskItem task)
    {
        _context.Tasks.Remove(task);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Persists all changes made in the database.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}