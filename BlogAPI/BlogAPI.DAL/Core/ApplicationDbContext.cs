using BlogAPI.Domain.Entity.Connection;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DAL.Core;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> User { get; set; }
    public DbSet<RoleEntity> Role { get; set; }
    public DbSet<UserRoleEntity> UserRole { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().ToTable("Users");
        modelBuilder.Entity<RoleEntity>().ToTable("Roles");
        modelBuilder.Entity<UserRoleEntity>().ToTable("UsersRoles");
        
        modelBuilder.Entity<UserEntity>().HasKey(user => user.UserId);
        modelBuilder.Entity<RoleEntity>().HasKey(role => role.RoleId);

        modelBuilder.Entity<UserEntity>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<RoleEntity>().HasIndex(role => role.RoleName).IsUnique();
        
        modelBuilder.Entity<RoleEntity>(builder =>
        {
            builder.Property(role => role.RoleName).HasMaxLength(20).IsRequired();
        });
        
        
        
        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.Property(user => user.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(user => user.LastName).HasMaxLength(50).IsRequired();
            builder.Property(user => user.Email).HasMaxLength(255).IsRequired();
            builder.Property(user => user.PasswordHash).IsRequired();
            builder.Property(user => user.PasswordSalt).IsRequired();
        });
        
        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { RoleId = 1, RoleName = "Administrator" },
            new RoleEntity { RoleId = 2, RoleName = "Author" },
            new RoleEntity { RoleId = 3, RoleName = "User" }
        );

        modelBuilder.Entity<UserRoleEntity>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(ur => ur.UserRole)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(ur => ur.UserRole)
            .HasForeignKey(ur => ur.RoleId);
    }
}