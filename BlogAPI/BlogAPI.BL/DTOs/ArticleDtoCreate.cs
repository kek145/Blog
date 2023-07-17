using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.BL.DTOs;

public class ArticleDtoCreate
{
    [Required, MinLength(5)]
    public string Title { get; set; } = string.Empty;
    [Required, MinLength(50)]
    public string Content { get; set; } = string.Empty;
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}