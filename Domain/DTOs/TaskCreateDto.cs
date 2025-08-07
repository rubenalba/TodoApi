using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class TaskCreateDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
}