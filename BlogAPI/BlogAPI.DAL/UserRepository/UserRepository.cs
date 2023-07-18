using System.Linq;
using BlogAPI.DAL.Core;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddUserAsync(UserEntity entity)
    {
        await _context.User.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UserEntity entity)
    {
        _context.User.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(UserEntity entity)
    {
        _context.User.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity> FindUserByIdAsync(int userId)
    {
        var user = await _context.User.FindAsync(userId);
        return user;
    }

    public async Task<UserEntity> FindUserByEmailAsync(string email)
    {
        var user = await _context.User.Where(find => find.Email == email).FirstOrDefaultAsync();
        return user;
    }
}