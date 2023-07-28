using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Domain.DTOs.CommentDTOs;

public class CommentUpdateDto
{
    [Required, MinLength(10)]
    public string Comment { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}