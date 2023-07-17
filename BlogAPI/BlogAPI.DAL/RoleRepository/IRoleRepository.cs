using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.RoleRepository;

public interface IRoleRepository
{
    Task<RoleEntity> GetRoleByIdAsync(int roleId);
    Task<RoleEntity> GetRoleByNameAsync(string roleName);
}