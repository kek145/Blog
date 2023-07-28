using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DAL.DTOs.EditUserDto;

public class UpdateUserDto
{
    [Required, MinLength(2)]
    public string FirstName { get; set; } = string.Empty;
    [Required, MinLength(2)]
    public string LastName { get; set; } = string.Empty;
}