using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.Domain.Entity.Table;

public class CommentEntity
{
    public int CommentId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [JsonIgnore]
    public ICollection<UserCommentEntity> UserComment { get; set; } = null!;
    [JsonIgnore]
    public ICollection<ArticleCommentEntity> ArticleComment { get; set; } = null!;
}