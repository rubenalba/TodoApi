using Domain.DTOs;

namespace Aplication.Interfaces;

/// <summary>
///     Interface defining the service layer operations for managing tasks.
///     Provides methods to retrieve, create, update, and delete <see cref="TaskReadDto"/> entities.
/// </summary>
public interface ITaskService
{
    /// <summary>
    ///     Retrieves all tasks for a specific user.
    /// </summary>
    /// <param name="id">The ID of the user whose tasks are retrieved.</param>
    /// <returns>A collection of <see cref="TaskReadDto"/>.</returns>
    Task<IEnumerable<TaskReadDto>> GetAllAsync(int id);

    /// <summary>
    ///     Retrieves a specific task by its ID for a given user.
    /// </summary>
    /// <param name="id">The ID of the task to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns>The <see cref="TaskReadDto"/> if found, or <c>null</c> otherwise.</returns>
    Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId);

    /// <summary>
    ///     Creates a new task for a specific user.
    /// </summary>
    /// <param name="task">The <see cref="TaskCreateDto"/> containing the task information.</param>
    /// <param name="userId">The ID of the user creating the task.</param>
    /// <returns>The created <see cref="TaskReadDto"/>.</returns>
    Task<TaskReadDto> CreateTaskAsync(TaskCreateDto task, int userId);

    /// <summary>
    ///     Updates an existing task by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="task">The <see cref="TaskUpdateDto"/> containing updated task data.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    Task<bool> UpdateTaskAsync(int id, TaskUpdateDto task, int userId);

    /// <summary>
    ///     Deletes a task by its ID for a specific user.
    /// </summary>
    /// <param name="i">The ID of the task to delete.</param>
    /// <param name="id">The ID of the user who owns the task.</param>
    /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
    Task<bool> DeleteTaskAsync(int i, int id);
}