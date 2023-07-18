using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.Domain.Entity.Connection;

public class ArticleCommentEntity
{
    public int ArticleId { get; set; }
    public ArticleEntity Article { get; set; } = null!;
    public int CommentId { get; set; }
    public CommentEntity Comment { get; set; } = null!;
}