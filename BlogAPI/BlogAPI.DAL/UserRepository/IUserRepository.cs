using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.UserRepository;

public interface IUserRepository
{
    Task AddNewUserAsync(UserEntity entity);
    Task UpdateUserAsync(UserEntity entity);
    Task DeleteUserAsync(UserEntity entity);
    Task<UserEntity> FindUserByIdAsync(int userId);
    Task<UserEntity> FindUserByEmailAsync(string email);
}