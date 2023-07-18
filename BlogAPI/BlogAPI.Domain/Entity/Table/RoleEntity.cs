using System.Collections.Generic;
using System.Text.Json.Serialization;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.Domain.Entity.Table;

public class RoleEntity
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    [JsonIgnore]
    public ICollection<UserRoleEntity> UserRole { get; set; } = null!;
}