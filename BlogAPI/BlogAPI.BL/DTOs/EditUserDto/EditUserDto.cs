using System.ComponentModel.DataAnnotations;

namespace BlogAPI.BL.DTOs.EditUserDto;

public class EditUserDto
{
    [Required, MinLength(2)]
    public string FirstName { get; set; } = string.Empty;
    [Required, MinLength(2)]
    public string LastName { get; set; } = string.Empty;
}