using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Domain.DTOs.RegistrationDto;

public class RegistrationDto
{
    [Required, MinLength(2)]
    public string FirstName { get; set; } = string.Empty;
    [Required, MinLength(2)]
    public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string ConfirmPassword { get; set; } = string.Empty;
    [Required, MinLength(4)]
    public string Role { get; set; } = string.Empty;
}