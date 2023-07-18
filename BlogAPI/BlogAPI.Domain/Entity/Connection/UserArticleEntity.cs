using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.Domain.Entity.Connection;

public class UserArticleEntity
{
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public int ArticleId { get; set; }
    public ArticleEntity Article { get; set; } = null!;
}