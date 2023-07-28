using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class UserCommentRepository : IRelationShipRepository<UserCommentEntity>
{
    private readonly ApplicationDbContext _context;

    public UserCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRelationShipAsync(UserCommentEntity entity)
    {
        await _context.UserComment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}