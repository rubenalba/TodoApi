namespace Domain.DTOs;

/// <summary>
///     Data Transfer Object (DTO) used to read task information.
///     Represents a <see cref="Domain.Models.TaskItem"/> for returning data to clients.
/// </summary>
public class TaskReadDto
{
    /// <summary>
    ///     The unique identifier of the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Indicates whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    ///     The date and time when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}