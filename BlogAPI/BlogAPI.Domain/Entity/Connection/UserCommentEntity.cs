using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.Domain.Entity.Connection;

public class UserCommentEntity
{
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public int CommentId { get; set; }
    public CommentEntity Comment { get; set; } = null!;
}