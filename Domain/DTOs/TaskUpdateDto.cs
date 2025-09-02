using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

/// <summary>
///     Data Transfer Object (DTO) used to update an existing task.
///     Represents the information that can be modified in a <see cref="Domain.Models.TaskItem"/>.
/// </summary>
public class TaskUpdateDto
{
    /// <summary>
    ///     The title of the task.
    /// </summary>
    /// <remarks>
    ///     Required. Must have between 3 and 100 characters.
    /// </remarks>
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Indicates whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}