namespace TodoApi.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } =  string.Empty;
    public ICollection<TaskItem> tasks { get; set; } = new List<TaskItem>();
    
}