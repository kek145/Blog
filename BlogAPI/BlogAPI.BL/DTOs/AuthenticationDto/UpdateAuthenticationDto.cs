using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.BL.DTOs.AuthenticationDto;

public class UpdateAuthenticationDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
    [Required, MinLength(6)]
    public string ConfirmPassword { get; set; } = string.Empty;
}