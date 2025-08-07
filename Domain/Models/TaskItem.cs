namespace Domain.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } =  String.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int UserId { get; set; }
    public User? User { get; set; }
}