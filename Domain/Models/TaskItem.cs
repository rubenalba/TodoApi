namespace Domain.Models;

/// <summary>
///     Represents a task in the system.
///     This is the domain model corresponding to tasks stored in the database.
/// </summary>
public class TaskItem
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
    ///     Indicates whether the task has been completed.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    ///     The date and time when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     The ID of the user who owns this task.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///     The user who owns this task.
    /// </summary>
    public User? User { get; set; }
}