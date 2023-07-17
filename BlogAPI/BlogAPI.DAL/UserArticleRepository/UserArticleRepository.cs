using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.UserArticleRepository;

public class UserArticleRepository : IUserArticleRepository
{
    private readonly ApplicationDbContext _context;

    public UserArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserArticleAsync(UserArticleEntity entity)
    {
        await _context.UserArticle.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserArticleAsync(UserArticleEntity entity)
    {
        _context.UserArticle.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserArticleEntity> FindUserArticleById(int userId, int articleId)
    {
        var userArticle = await _context.UserArticle.Where(find => find.UserId == userId && find.ArticleId == articleId).FirstOrDefaultAsync();
        return userArticle!;
    }
}