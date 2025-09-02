using Domain.Models;

namespace Aplication.Interfaces;

/// <summary>
///     Interface defining the operations for managing <see cref="TaskItem"/> entities in the data store.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    ///     Retrieves all tasks for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose tasks are retrieved.</param>
    /// <returns>A collection of <see cref="TaskItem"/>.</returns>
    Task<IEnumerable<TaskItem>> GetAllAsync(int userId);

    /// <summary>
    ///     Retrieves a task by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns>The <see cref="TaskItem"/> if found, or <c>null</c> otherwise.</returns>
    Task<TaskItem?> GetByIdAsync(int id, int userId);

    /// <summary>
    ///     Adds a new <see cref="TaskItem"/> to the data store.
    /// </summary>
    /// <param name="task">The task to add.</param>
    Task AddAsync(TaskItem task);

    /// <summary>
    ///     Updates an existing <see cref="TaskItem"/> in the data store.
    /// </summary>
    /// <param name="task">The task to update.</param>
    Task UpdateAsync(TaskItem task);

    /// <summary>
    ///     Deletes an existing <see cref="TaskItem"/> from the data store.
    /// </summary>
    /// <param name="task">The task to delete.</param>
    Task DeleteAsync(TaskItem task);

    /// <summary>
    ///     Persists all changes made in the data store.
    /// </summary>
    Task SaveChangesAsync();
}