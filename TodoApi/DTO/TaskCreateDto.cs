using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTO;

public class TaskCreateDto
{
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty;
}