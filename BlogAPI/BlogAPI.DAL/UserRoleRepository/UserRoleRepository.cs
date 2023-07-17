using System.Linq;
using System.Threading.Tasks;
using BlogAPI.DAL.Core;
using BlogAPI.Domain.Entity.Connection;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.UserRoleRepository;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddNewUserRoleAsync(UserRoleEntity entity)
    {
        await _context.UserRole.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserRoleAsync(UserRoleEntity entity)
    {
        _context.UserRole.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<UserRoleEntity> GetUserRoleAsync(int userId, int roleId)
    {
        var userRole = await _context.UserRole.Where(find => find.UserId == userId && find.RoleId == roleId).FirstOrDefaultAsync();
        return userRole;
    }
}