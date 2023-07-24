using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Repositories;

public class RoleRepository : IBaseRepository<RoleEntity>
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RoleEntity entity)
    {
        await _context.Role.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<RoleEntity> GetAll()
    {
        return _context.Role;
    }

    public async Task<RoleEntity> UpdateAsync(RoleEntity entity)
    {
        _context.Role.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<List<RoleEntity>> GetAllById(int entityId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<RoleEntity>> DeleteAllAsync(IEnumerable<RoleEntity> entity)
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(RoleEntity entity)
    {
        _context.Role.Remove(entity);
        await _context.SaveChangesAsync();
    }
}