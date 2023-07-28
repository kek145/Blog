using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.Domain.Entity.Connection;

public class ArticleCategoryEntity
{
    public int ArticleId { get; set; }
    public ArticleEntity? Article { get; set; } = null!;
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;
}