using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.RoleRepository;

public interface IRoleRepository
{
    Task<RoleEntity> FindRoleByIdAsync(int roleId);
    Task<string> FindRolesByUserIdAsync(int userId);
    Task<RoleEntity> FindRoleByNameAsync(string roleName);
}