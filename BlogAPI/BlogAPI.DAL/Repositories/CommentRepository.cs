using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using System.Collections.Generic;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.Repositories;

public class CommentRepository : IGenericRepository<CommentEntity>, IQueryAndDeleteRepository<CommentEntity>
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CommentEntity entity)
    {
        await _context.Comment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<CommentEntity> GetAll()
    {
        return _context.Comment;
    }

    public async Task<CommentEntity> UpdateAsync(CommentEntity entity)
    {
        _context.Comment.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
    
    public async Task DeleteAsync(CommentEntity entity)
    {
        _context.Comment.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CommentEntity>> GetAllById(int articleId)
    {
        var comments = await GetAll()
            .Where(find => find.ArticleComment.Any(commentsArticle => commentsArticle.ArticleId == articleId))
            .ToListAsync();

        return comments;
    }

    public async Task<List<CommentEntity>> GetAllByUserId(int userId)
    {
        var comments = await GetAll()
            .Where(find => find.UserComment.Any(userComments => userComments.UserId == userId))
            .ToListAsync();
        return comments;
    }

    public async Task<List<CommentEntity>> DeleteAllAsync(IEnumerable<CommentEntity> entity)
    {
        _context.Comment.RemoveRange(entity);
        await _context.SaveChangesAsync();

        return await _context.Comment.ToListAsync();
    }
}