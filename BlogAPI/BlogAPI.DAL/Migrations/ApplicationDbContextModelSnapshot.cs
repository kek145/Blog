﻿// <auto-generated />
using System;
using BlogAPI.DAL.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogAPI.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.ArticleCategoryEntity", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("ArticleId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ArticlesCategories", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.ArticleCommentEntity", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.HasKey("ArticleId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("ArticlesComments", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.UserArticleEntity", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ArticleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersArticles", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.UserCommentEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("UsersComments", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.UserRoleEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UsersRoles", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.ArticleEntity", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2023, 7, 25, 19, 8, 59, 952, DateTimeKind.Utc).AddTicks(2323));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ArticleId");

                    b.ToTable("Articles", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.CategoryEntity", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoryId");

                    b.HasIndex("CategoryName")
                        .IsUnique();

                    b.ToTable("Categories", (string)null);

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryName = "News"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryName = "Sport"
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryName = "Games"
                        });
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.CommentEntity", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2023, 7, 25, 19, 8, 59, 952, DateTimeKind.Utc).AddTicks(1892));

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("CommentId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.RoleEntity", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleName = "Administrator"
                        },
                        new
                        {
                            RoleId = 2,
                            RoleName = "Author"
                        },
                        new
                        {
                            RoleId = 3,
                            RoleName = "User"
                        });
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.ArticleCategoryEntity", b =>
                {
                    b.HasOne("BlogAPI.Domain.Entity.Table.ArticleEntity", "Article")
                        .WithMany("ArticleCategory")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAPI.Domain.Entity.Table.CategoryEntity", "Category")
                        .WithMany("ArticleCategory")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.ArticleCommentEntity", b =>
                {
                    b.HasOne("BlogAPI.Domain.Entity.Table.ArticleEntity", "Article")
                        .WithMany("ArticleComment")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAPI.Domain.Entity.Table.CommentEntity", "Comment")
                        .WithMany("ArticleComment")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.UserArticleEntity", b =>
                {
                    b.HasOne("BlogAPI.Domain.Entity.Table.ArticleEntity", "Article")
                        .WithMany("UserArticle")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAPI.Domain.Entity.Table.UserEntity", "User")
                        .WithMany("UserArticle")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.UserCommentEntity", b =>
                {
                    b.HasOne("BlogAPI.Domain.Entity.Table.CommentEntity", "Comment")
                        .WithMany("UserComment")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAPI.Domain.Entity.Table.UserEntity", "User")
                        .WithMany("UserComment")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Connection.UserRoleEntity", b =>
                {
                    b.HasOne("BlogAPI.Domain.Entity.Table.RoleEntity", "Role")
                        .WithMany("UserRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogAPI.Domain.Entity.Table.UserEntity", "User")
                        .WithMany("UserRole")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.ArticleEntity", b =>
                {
                    b.Navigation("ArticleCategory");

                    b.Navigation("ArticleComment");

                    b.Navigation("UserArticle");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.CategoryEntity", b =>
                {
                    b.Navigation("ArticleCategory");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.CommentEntity", b =>
                {
                    b.Navigation("ArticleComment");

                    b.Navigation("UserComment");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.RoleEntity", b =>
                {
                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("BlogAPI.Domain.Entity.Table.UserEntity", b =>
                {
                    b.Navigation("UserArticle");

                    b.Navigation("UserComment");

                    b.Navigation("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}
