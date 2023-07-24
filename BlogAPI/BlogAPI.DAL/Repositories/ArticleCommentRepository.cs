using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class ArticleCommentRepository : IBaseRepository<ArticleCommentEntity>
{
    private readonly ApplicationDbContext _context;

    public ArticleCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ArticleCommentEntity entity)
    {
        await _context.ArticleComment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<ArticleCommentEntity> GetAll()
    {
        return _context.ArticleComment;
    }

    public async Task<ArticleCommentEntity> UpdateAsync(ArticleCommentEntity entity)
    {
        _context.ArticleComment.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<List<ArticleCommentEntity>> GetAllById(int entityId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<ArticleCommentEntity>> DeleteAllAsync(IEnumerable<ArticleCommentEntity> entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(ArticleCommentEntity entity)
    {
        _context.ArticleComment.Remove(entity);
        await _context.SaveChangesAsync();
    }
}