using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class UserArticleRepository : IRelationShipRepository<UserArticleEntity>
{
    private readonly ApplicationDbContext _context;

    public UserArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRelationShipAsync(UserArticleEntity entity)
    {
        await _context.UserArticle.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}