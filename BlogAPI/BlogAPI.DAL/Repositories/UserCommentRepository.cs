using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class UserCommentRepository : IBaseRepository<UserCommentEntity>
{
    private readonly ApplicationDbContext _context;

    public UserCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserCommentEntity entity)
    {
        await _context.UserComment.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<UserCommentEntity> GetAll()
    {
        return _context.UserComment;
    }

    public async Task<UserCommentEntity> UpdateAsync(UserCommentEntity entity)
    {
        _context.UserComment.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<List<UserCommentEntity>> GetAllById(int entityId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<UserCommentEntity>> DeleteAllAsync(IEnumerable<UserCommentEntity> entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(UserCommentEntity entity)
    {
        _context.UserComment.Remove(entity);
        await _context.SaveChangesAsync();
    }
}