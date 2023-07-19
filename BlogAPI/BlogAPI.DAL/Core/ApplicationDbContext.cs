using System;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.Core;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> User { get; set; } = null!;
    public DbSet<RoleEntity> Role { get; set; } = null!;
    public DbSet<CommentEntity> Comment { get; set; } = null!;
    public DbSet<ArticleEntity> Article { get; set; } = null!;
    public DbSet<CategoryEntity> Category { get; set; } = null!;

    public DbSet<UserRoleEntity> UserRole { get; set; } = null!;
    public DbSet<UserArticleEntity> UserArticle { get; set; } = null!;
    public DbSet<UserCommentEntity> UserComment { get; set; } = null!;
    public DbSet<ArticleCommentEntity> ArticleComment { get; set; } = null!;
    public DbSet<ArticleCategoryEntity> ArticleCategory { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().ToTable("Users");
        modelBuilder.Entity<RoleEntity>().ToTable("Roles");
        modelBuilder.Entity<CommentEntity>().ToTable("Comments");
        modelBuilder.Entity<ArticleEntity>().ToTable("Articles");
        modelBuilder.Entity<CategoryEntity>().ToTable("Categories");

        modelBuilder.Entity<UserRoleEntity>().ToTable("UsersRoles");
        modelBuilder.Entity<UserArticleEntity>().ToTable("UsersArticles");
        modelBuilder.Entity<UserCommentEntity>().ToTable("UsersComments");
        modelBuilder.Entity<ArticleCommentEntity>().ToTable("ArticlesComments");
        modelBuilder.Entity<ArticleCategoryEntity>().ToTable("ArticlesCategories");

        modelBuilder.Entity<UserEntity>().HasKey(user => user.UserId);
        modelBuilder.Entity<RoleEntity>().HasKey(role => role.RoleId);
        modelBuilder.Entity<CommentEntity>().HasKey(comment => comment.CommentId);
        modelBuilder.Entity<ArticleEntity>().HasKey(article => article.ArticleId);
        modelBuilder.Entity<CategoryEntity>().HasKey(category => category.CategoryId);

        modelBuilder.Entity<UserEntity>().HasIndex(user => user.Email).IsUnique();
        modelBuilder.Entity<RoleEntity>().HasIndex(role => role.RoleName).IsUnique();
        modelBuilder.Entity<CategoryEntity>().HasIndex(category => category.CategoryName).IsUnique();

        modelBuilder.Entity<RoleEntity>(builder =>
        {
            builder.Property(role => role.RoleName).HasMaxLength(20).IsRequired();
        });
        modelBuilder.Entity<CategoryEntity>(builder =>
        {
            builder.Property(comment => comment.CategoryName).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<CommentEntity>(builder =>
        {
            builder.Property(comment => comment.Comment).HasMaxLength(2000).IsRequired();
            builder.Property(comment => comment.CreatedAt).HasDefaultValue(DateTime.UtcNow);
        });
        
        modelBuilder.Entity<ArticleEntity>(builder =>
        {
            builder.Property(article => article.Title).HasMaxLength(200).IsRequired();
            builder.Property(article => article.Content).HasMaxLength(4000).IsRequired();
            builder.Property(article => article.CreatedAt).HasDefaultValue(DateTime.UtcNow);
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

        modelBuilder.Entity<CategoryEntity>().HasData(
            new CategoryEntity { CategoryId = 1, CategoryName = "News" },
            new CategoryEntity { CategoryId = 2, CategoryName = "Sport" },
            new CategoryEntity { CategoryId = 3, CategoryName = "Games" }
        );
        
        modelBuilder.Entity<UserRoleEntity>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.UserRole)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(ur => ur.UserRole)
            .HasForeignKey(ur => ur.RoleId);
            
        modelBuilder.Entity<UserArticleEntity>()
            .HasKey(ua => new { ua.ArticleId, ua.UserId });
            
        modelBuilder.Entity<UserArticleEntity>()
            .HasOne(ua => ua.Article)
            .WithMany(ua => ua.UserArticle)
            .HasForeignKey(ua => ua.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<UserArticleEntity>()
            .HasOne(ua => ua.User)
            .WithMany(ua => ua.UserArticle)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<UserCommentEntity>()
            .HasKey(uc => new { uc.UserId, uc.CommentId });
            
        modelBuilder.Entity<UserCommentEntity>()
            .HasOne(uc => uc.User)
            .WithMany(uc => uc.UserComment)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<UserCommentEntity>()
            .HasOne(uc => uc.Comment)
            .WithMany(uc => uc.UserComment)
            .HasForeignKey(uc => uc.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ArticleCommentEntity>()
            .HasKey(ac => new { ac.ArticleId, ac.CommentId });
            
        modelBuilder.Entity<ArticleCommentEntity>()
            .HasOne(ac => ac.Comment)
            .WithMany(ac => ac.ArticleComment)
            .HasForeignKey(ac => ac.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ArticleCommentEntity>()
            .HasOne(ac => ac.Article)
            .WithMany(ac => ac.ArticleComment)
            .HasForeignKey(ac => ac.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ArticleCategoryEntity>()
            .HasKey(ac => new { ac.ArticleId, ac.CategoryId });
            
        modelBuilder.Entity<ArticleCategoryEntity>()
            .HasOne(ac => ac.Article)
            .WithMany(ac => ac.ArticleCategory)
            .HasForeignKey(ac => ac.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ArticleCategoryEntity>()
            .HasOne(ac => ac.Category)
            .WithMany(ac => ac.ArticleCategory)
            .HasForeignKey(ac => ac.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}