using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.Domain.Entity.Connection;

public class UserRoleEntity
{
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public int RoleId { get; set; }
    public RoleEntity Role { get; set; }
}