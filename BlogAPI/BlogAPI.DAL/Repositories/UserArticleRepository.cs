using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class UserArticleRepository : IBaseRepository<UserArticleEntity>
{
    private readonly ApplicationDbContext _context;

    public UserArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserArticleEntity entity)
    {
        await _context.UserArticle.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<UserArticleEntity> GetAll()
    {
        return _context.UserArticle;
    }

    public async Task<UserArticleEntity> UpdateAsync(UserArticleEntity entity)
    {
        _context.UserArticle.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<List<UserArticleEntity>> GetAllById(int entityId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<UserArticleEntity>> DeleteAllAsync(IEnumerable<UserArticleEntity> entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(UserArticleEntity entity)
    {
        _context.UserArticle.Remove(entity);
        await _context.SaveChangesAsync();
    }
}