using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DAL.DTOs.ArticleDTOs;

public class ArticleCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    [Required]
    public string Category { get; set; } = string.Empty;
}