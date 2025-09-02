namespace Domain.Models;

/// <summary>
///     Represents a user in the system.
///     This is the domain model corresponding to users stored in the database.
/// </summary>
public class User
{
    /// <summary>
    ///     The unique identifier of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     The hashed password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     The collection of tasks that belong to the user.
    /// </summary>
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}