using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Domain.DTOs.AuthenticationDto;

public class AuthenticationDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
}