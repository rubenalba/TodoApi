using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class TaskUpdateDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } =  string.Empty;
    public bool IsCompleted { get; set; }
}