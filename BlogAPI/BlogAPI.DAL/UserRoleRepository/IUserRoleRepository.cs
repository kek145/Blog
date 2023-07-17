using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.UserRoleRepository;

public interface IUserRoleRepository
{
    Task AddNewUserRoleAsync(UserRoleEntity entity);
    Task DeleteUserRoleAsync(UserRoleEntity entity);
    Task<UserRoleEntity> GetUserRoleAsync(int userId, int roleId);
}