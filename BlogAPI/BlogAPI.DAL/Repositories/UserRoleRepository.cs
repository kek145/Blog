﻿using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Repositories;

public class UserRoleRepository : IGenericRepository<UserRoleEntity>
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserRoleEntity entity)
    {
        await _context.UserRole.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<UserRoleEntity> GetAll()
    {
        return _context.UserRole;
    }

    public async Task<UserRoleEntity> UpdateAsync(UserRoleEntity entity)
    {
        _context.UserRole.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(UserRoleEntity entity)
    {
        _context.UserRole.Remove(entity);
        await _context.SaveChangesAsync();
    }
}