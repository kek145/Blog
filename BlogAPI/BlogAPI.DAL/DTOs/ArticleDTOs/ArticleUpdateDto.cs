using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DAL.DTOs.ArticleDTOs;

public class ArticleUpdateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
}