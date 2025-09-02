using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

/// <summary>
///     Data Transfer Object (DTO) used to create a new task.
///     Contains the necessary information to create a <see cref="Domain.Models.TaskItem"/>.
/// </summary>
public class TaskCreateDto
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
}