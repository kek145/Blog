using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.Domain.Entity.Table;

public class ArticleEntity
{
    public int ArticleId { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [JsonIgnore]
    public ICollection<UserArticleEntity> UserArticle { get; set; } = null!;
    [JsonIgnore]
    public ICollection<ArticleCommentEntity> ArticleComment { get; set; } = null!;
    [JsonIgnore]
    public ICollection<ArticleCategoryEntity> ArticleCategory { get; set; } = null!;
}