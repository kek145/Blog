using System.Collections.Generic;
using System.Text.Json.Serialization;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.Domain.Entity.Table;

public class UserEntity
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    [JsonIgnore]
    public ICollection<UserRoleEntity> UserRole { get; set; } = null!;
    [JsonIgnore]
    public ICollection<UserArticleEntity> UserArticle { get; set; } = null!;
    [JsonIgnore]
    public ICollection<UserCommentEntity> UserComment { get; set; } = null!;
}