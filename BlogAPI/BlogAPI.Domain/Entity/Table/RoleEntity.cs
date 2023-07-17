using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.Domain.Entity.Table;

public class RoleEntity
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public ICollection<UserRoleEntity> UserRole { get; set; }
}