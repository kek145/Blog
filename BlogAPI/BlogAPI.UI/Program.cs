using System;
using System.Text;
using BlogAPI.DAL.Core;
using BlogAPI.DAL.Interfaces;
using Microsoft.OpenApi.Models;
using BlogAPI.DAL.Repositories;
using BlogAPI.BL.ArticleService;
using BlogAPI.BL.AccountService;
using BlogAPI.BL.CommentService;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.Domain.Entity.Table;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BlogAPI.BL.RegistrationService;
using Swashbuckle.AspNetCore.Filters;
using BlogAPI.BL.AuthenticationService;
using BlogAPI.Domain.Entity.Connection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

var secretKey = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTConfiguration:SecretKey").Value!);
var issuer = builder.Configuration.GetSection("JWTConfiguration:Issuer").Value;
var audience = builder.Configuration.GetSection("JWTConfiguration:Audience").Value;

var tokenValidationParameter = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = issuer,
    ValidateAudience = true,
    ValidAudience = audience,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    RequireExpirationTime = true,
    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
};

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IGenericRepository<CommentEntity>, CommentRepository>();
builder.Services.AddScoped<IGenericRepository<ArticleEntity>, ArticleRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IBaseRepository<UserEntity>, UserRepository>();
builder.Services.AddScoped<IBaseRepository<RoleEntity>, RoleRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IBaseRepository<ArticleEntity>, ArticleRepository>();
builder.Services.AddScoped<IBaseRepository<CommentEntity>, CommentRepository>();
builder.Services.AddScoped<IBaseRepository<CategoryEntity>, CategoryRepository>();
builder.Services.AddScoped<IBaseRepository<UserRoleEntity>, UserRoleRepository>();
builder.Services.AddScoped<IBaseRepository<UserArticleEntity>, UserArticleRepository>();
builder.Services.AddScoped<IBaseRepository<UserCommentEntity>, UserCommentRepository>();
builder.Services.AddScoped<IBaseRepository<ArticleCommentEntity>, ArticleCommentRepository>();
builder.Services.AddScoped<IBaseRepository<ArticleCategoryEntity>, ArticleCategoryRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme(\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParameter;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();