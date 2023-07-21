using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.UserCommentRepository;

public class UserCommentRepository : IUserCommentRepository
{
    private readonly ApplicationDbContext _context;

    public UserCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddUserCommentAsync(UserCommentEntity entity)
    {
        await _context.UserComment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}