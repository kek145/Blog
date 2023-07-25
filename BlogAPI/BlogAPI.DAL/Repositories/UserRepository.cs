using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using System.Collections.Generic;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Repositories;

public class UserRepository : IGenericRepository<UserEntity>
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(UserEntity entity)
    {
        await _context.User.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<UserEntity> GetAll()
    {
        return _context.User;
    }

    public async Task<UserEntity> UpdateAsync(UserEntity entity)
    {
        _context.User.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<List<UserEntity>> GetAllById(int entityId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<UserEntity>> DeleteAllAsync(IEnumerable<UserEntity> entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(UserEntity entity)
    {
        _context.User.Remove(entity);
        await _context.SaveChangesAsync();
    }
}