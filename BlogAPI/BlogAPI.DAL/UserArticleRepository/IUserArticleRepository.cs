using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.UserArticleRepository;

public interface IUserArticleRepository
{
    Task AddUserArticleAsync(UserArticleEntity entity);
    Task DeleteUserArticleAsync(UserArticleEntity entity);
    Task<UserArticleEntity> FindUserArticleByIdAsync(int userId, int articleId);
}