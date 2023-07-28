using System;

namespace BlogAPI.DAL.DTOs.CommentDTOs;

public class CommentDto
{
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}