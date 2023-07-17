using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.UserArticleRepository;

public interface IUserArticleRepository
{
    Task CreateUserArticleAsync(UserArticleEntity entity);
    Task DeleteUserArticleAsync(UserArticleEntity entity);
    Task<UserArticleEntity> FindUserArticleById(int userId, int articleId);
}