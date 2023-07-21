using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.BL.DTOs.CommentDTOs;

public class CommentUpdateDto
{
    [Required, MinLength(10)]
    public string Comment { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}