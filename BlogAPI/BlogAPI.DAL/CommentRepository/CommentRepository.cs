using System.Linq;
using BlogAPI.DAL.Core; 
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.CommentRepository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<CommentEntity> GetAllComments()
    {
        return _context.Comment;
    }

    public async Task AddCommentAsync(CommentEntity entity)
    {
        await _context.Comment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(CommentEntity entity)
    {
        _context.Comment.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<CommentEntity> UpdateCommentAsync(CommentEntity entity)
    {
        _context.Comment.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}