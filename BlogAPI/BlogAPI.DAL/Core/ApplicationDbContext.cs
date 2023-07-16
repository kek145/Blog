using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.Core;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
}