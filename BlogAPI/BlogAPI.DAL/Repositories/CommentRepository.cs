using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Repositories;

public class CommentRepository : IBaseRepository<CommentEntity>
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
}