using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.DTOs;
using Domain.Models;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

/// <summary>
///     Controller responsible for handling authentication-related operations such as login and registration.
///     Provides endpoints to generate JWT tokens and create new users.
/// </summary>
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly TodoDbContext _context;
    private readonly IConfiguration _config;

    /// <summary>
    ///     Initializes a new instance of <see cref="AuthController"/>.
    /// </summary>
    /// <param name="context">The database context <see cref="TodoDbContext"/>.</param>
    /// <param name="config">The application configuration <see cref="IConfiguration"/>.</param>
    public AuthController(TodoDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    /// <summary>
    ///     Authenticates a user with email and password and returns a JWT token.
    /// </summary>
    /// <param name="dto">The login information as <see cref="LoginDto"/>.</param>
    /// <returns>
    ///     <see cref="OkObjectResult"/> containing the JWT token if successful,
    ///     or <see cref="UnauthorizedResult"/> if authentication fails.
    /// </returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if(user == null) return Unauthorized("User not found");
        if(user.Password != HashPassword(dto.Password)) return Unauthorized("Wrong password");
        var token = GenerateJwtToken(user);
        return Ok(token);
    }

    /// <summary>
    ///     Registers a new user in the system.
    /// </summary>
    /// <param name="dto">The registration information as <see cref="RegisterDto"/>.</param>
    /// <returns>
    ///     <see cref="OkObjectResult"/> if the user was successfully created,
    ///     or <see cref="BadRequestObjectResult"/> if the email is already taken.
    /// </returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest("Email is already taken");
        }

        var user = new User
        {
            Email = dto.Email,
            Password = HashPassword(dto.Password),
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User created");
    }

    /// <summary>
    ///     Hashes a plain text password using SHA256.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>The hashed password as a Base64 string.</returns>
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    ///     Generates a JWT token for a given <see cref="User"/>.
    /// </summary>
    /// <param name="user">The user to generate the token for.</param>
    /// <returns>A JWT token as <see cref="string"/>.</returns>
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}