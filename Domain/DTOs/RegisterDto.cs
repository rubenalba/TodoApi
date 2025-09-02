using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

/// <summary>
///     Data Transfer Object (DTO) used for registering a new user.
///     Contains the necessary information to create a user account.
/// </summary>
public class RegisterDto
{
    /// <summary>
    ///     The email of the user.
    /// </summary>
    /// <remarks>
    ///     Must be a valid email address and is required.
    /// </remarks>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     The password for the new user account.
    /// </summary>
    /// <remarks>
    ///     Required and must have at least 6 characters.
    /// </remarks>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}