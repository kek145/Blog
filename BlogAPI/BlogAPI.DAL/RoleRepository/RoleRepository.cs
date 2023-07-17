using System.Linq;
using System.Threading.Tasks;
using BlogAPI.DAL.Core;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.RoleRepository;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<RoleEntity> GetRoleByIdAsync(int roleId)
    {
        var role = await _context.Role.FindAsync(roleId);
        return role;
    }

    public async Task<RoleEntity> GetRoleByNameAsync(string roleName)
    {
        var role = await _context.Role.Where(find => find.RoleName == roleName).FirstOrDefaultAsync();
        return role;
    }
}