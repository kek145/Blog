using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Domain.DTOs.CommentDTOs;

public class CommentAddDto
{
    [Required, MinLength(10)]
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}