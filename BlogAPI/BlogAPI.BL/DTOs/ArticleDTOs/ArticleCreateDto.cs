using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.BL.DTOs.ArticleDTOs;

public class ArticleCreateDto
{
    [Required, MinLength(5)]
    public string Title { get; set; } = string.Empty;
    [Required, MinLength(50)]
    public string Content { get; set; } = string.Empty;
    [Required, MinLength(4)]
    public string Category { get; set; } = string.Empty;
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}