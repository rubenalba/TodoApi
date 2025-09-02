using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data;

/// <summary>
///     Represents the Entity Framework database context for the Todo application.
///     Provides access to <see cref="TaskItem"/> and <see cref="User"/> entities.
/// </summary>
public class TodoDbContext : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TodoDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to configure the <see cref="DbContext"/>.</param>
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) {}

    /// <summary>
    ///     Gets or sets the tasks in the database.
    /// </summary>
    public DbSet<TaskItem> Tasks { get; set; }

    /// <summary>
    ///     Gets or sets the users in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }
}