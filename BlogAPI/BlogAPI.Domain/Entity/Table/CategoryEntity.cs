using System.Collections.Generic;
using System.Text.Json.Serialization;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.Domain.Entity.Table;

public class CategoryEntity
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    [JsonIgnore]
    public ICollection<ArticleCategoryEntity> ArticleCategory { get; set; } = null!;
}