using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

/// <summary>
///     Data Transfer Object (DTO) used for user login operations.
///     Contains the credentials required to authenticate a user.
/// </summary>
public class LoginDto
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
    ///     The password of the user.
    /// </summary>
    /// <remarks>
    ///     Required for authentication.
    /// </remarks>
    [Required]
    public string Password { get; set; } = string.Empty;
}