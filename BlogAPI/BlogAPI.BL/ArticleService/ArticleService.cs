using System;
using System.Linq;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.ArticleDTOs;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.BL.ArticleService;

public class ArticleService : IArticleService
{
    private readonly ILogger<ArticleService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICommentRepository _commentRepository;
    private readonly IBaseRepository<UserEntity> _userRepository;
    private readonly IBaseRepository<ArticleEntity> _articleRepository;
    private readonly IBaseRepository<CategoryEntity> _categoryRepository;
    private readonly IBaseRepository<UserArticleEntity> _userArticleRepository;
    private readonly IBaseRepository<ArticleCategoryEntity> _articleCategoryRepository;

    public ArticleService(
        ILogger<ArticleService> logger,
        IJwtTokenService jwtTokenService,
        ICommentRepository commentRepository,
        IBaseRepository<UserEntity> userRepository,
        IBaseRepository<ArticleEntity> articleRepository,
        IBaseRepository<CategoryEntity> categoryRepository,
        IBaseRepository<UserArticleEntity> userArticleRepository,
        IBaseRepository<ArticleCategoryEntity> articleCategoryRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _commentRepository = commentRepository;
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
        _userArticleRepository = userArticleRepository;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public async Task<IBaseResponse<ArticleEntity>> CreateNewArticleAsync(ArticleCreateDto articleDto, string token)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var category = await _categoryRepository.GetAll()
                .Where(find => find.CategoryName == articleDto.Category)
                .FirstOrDefaultAsync();
            var title = await _articleRepository.GetAll()
                .Where(find => find.Title == articleDto.Title)
                .FirstOrDefaultAsync();
            
            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized");
                return new BaseResponse<ArticleEntity>().ServerResponse("User is not authorized", StatusCode.Unauthorized);
            }
            
            if (category == null!)
            {
                _logger.LogError("This category does not exist");
                return new BaseResponse<ArticleEntity>().ServerResponse("This category does not exist.", StatusCode.NotFound);
            }

            if (title != null!)
            {
                _logger.LogError("There is already an article with the same title");
                return new BaseResponse<ArticleEntity>().ServerResponse("There is already an article with the same title.", StatusCode.Conflict);
            }

            var article = new ArticleEntity
            {
                Title = articleDto.Title,
                Content = articleDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            var userArticle = new UserArticleEntity
            {
                UserId = userId.Value,
                Article = article,
            };

            var articleCategory = new ArticleCategoryEntity
            {
                Article = article,
                Category = category
            };

            await _articleRepository.AddAsync(article);
            await _userArticleRepository.AddAsync(userArticle);
            await _articleCategoryRepository.AddAsync(articleCategory);

            _logger.LogInformation("The article has been successfully created!");
            return new BaseResponse<ArticleEntity>().ServerResponse("The article has been successfully created!", StatusCode.Created);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<IEnumerable<ArticleDto>>> GetAllArticlesAsync()
    {
        try
        {
            var articles = await _articleRepository.GetAll()
                .Select(x =>
                    new ArticleDto
                    {
                        Title = x.Title,
                        Content = x.Content,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }
                ).ToListAsync();
            return new BaseResponse<IEnumerable<ArticleDto>>
            {
                Data = articles,
                StatusCode = StatusCode.Ok,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<IEnumerable<ArticleDto>>> GetAllArticlesByUserAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetAll()
                .Where(find => find.UserId == userId && find.UserRole.Any(userRole => userRole.Role.RoleName == "Author"))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogError("User is not found");
                return new BaseResponse<ArticleDto>().ServerResponseEnumerable("User is not found!", StatusCode.NotFound);
            }

            var articles = await _articleRepository.GetAll()
                .Where(x => x.UserArticle.Any(userArticles => user.UserId == userId))
                .Select(x =>
                    new ArticleDto
                    {
                        Title = x.Title,
                        Content = x.Content,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                .ToListAsync();

            return new BaseResponse<IEnumerable<ArticleDto>>
            {
                Data = articles,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<IEnumerable<ArticleDto>>> GetArticleBySearchAsync(string query)
    {
        try
        {
            var articles = await _articleRepository.GetAll()
                .Where(x => x.Title.Contains(query) || x.Content.Contains(query))
                .Select(x =>
                    new ArticleDto
                    {
                        Title = x.Title,
                        Content = x.Content,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                .ToListAsync();
            return new BaseResponse<IEnumerable<ArticleDto>>
            {
                Data = articles,
                StatusCode = StatusCode.Ok,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<IEnumerable<ArticleDto>>
            {
                Description = "Internal server error.",
                StatusCode = StatusCode.InternalServerError
            };
        }
    }

    public async Task<IBaseResponse<ArticleDto>> GetArticleByIdAsync(int articleId)
    {
        try
        {
            var article = await _articleRepository.GetAll()
                .Where(find => find.ArticleId == articleId)
                .Select(x =>
                    new ArticleDto
                    {
                        Title = x.Title,
                        Content = x.Content,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    })
                .FirstOrDefaultAsync();

            if (article == null!)
            {
                _logger.LogError("Article is not found");
                return new BaseResponse<ArticleDto>().ServerResponse("Article is not found!", StatusCode.NotFound);
            }
            
            
            return new BaseResponse<ArticleDto>
            {
                Data = article,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleDto>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<IEnumerable<ArticleDto>>> GetAllArticlesByCategoryAsync(string categoryName)
    {
        try
        {
            var articles = await _articleRepository.GetAll()
                .Where(article => article.ArticleCategory
                    .Any(articleCategory => articleCategory.Category.CategoryName == categoryName))
                .Select(x => 
                    new ArticleDto
                    {
                        Title = x.Title,
                        Content = x.Content,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    }).ToListAsync();

            if (articles == null!)
            {
                _logger.LogError("Articles is not found!");
                return new BaseResponse<ArticleDto>().ServerResponseEnumerable("Articles is not found!", StatusCode.NotFound);
            }
            _logger.LogInformation("All articles!");
            return new BaseResponse<IEnumerable<ArticleDto>>
            {
                Data = articles,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<ArticleEntity>> UpdateArticleAsync(ArticleUpdateDto articleDto, string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.GetAll()
                .Where(find => find.ArticleId == articleId && find.UserArticle.Any(articleUser => articleUser.UserId == userId))
                .FirstOrDefaultAsync();
            var title = await _articleRepository.GetAll()
                .Where(find => find.Title == articleDto.Title)
                .FirstOrDefaultAsync();

            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized");
                return new BaseResponse<ArticleEntity>().ServerResponse("User is not authorized", StatusCode.Unauthorized);
            }
            
            if (article == null!)
            {
                _logger.LogError("Article not found!");
                return new BaseResponse<ArticleEntity>().ServerResponse("Article not found!", StatusCode.NotFound);
            }

            if (title != null!)
            {
                _logger.LogError("There is already an article with the same title!");
                return new BaseResponse<ArticleEntity>().ServerResponse("There is already an article with the same title.", StatusCode.Conflict);
            }

            article.Title = articleDto.Title;
            article.Content = articleDto.Content;
            article.UpdatedAt = DateTime.UtcNow;
            

            await _articleRepository.UpdateAsync(article);

            _logger.LogInformation("Article successfully updated!");
            return new BaseResponse<ArticleEntity>
            {
                Data = article,
                Description = "Article successfully updated!",
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }
    
    public async Task<IBaseResponse<ArticleEntity>> DeleteArticleAsync(string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            
            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized");
                return new BaseResponse<ArticleEntity>().ServerResponse("User is not authorized", StatusCode.Unauthorized);
            }
            
            var article = await _articleRepository.GetAll()
                .Where(find => find.ArticleId == articleId && find.UserArticle.Any(articleUser => articleUser.UserId == userId.Value))
                .FirstOrDefaultAsync();

            if (article == null!)
            {
                _logger.LogError("Article not found!");
                return new BaseResponse<ArticleEntity>().ServerResponse("Article not found!", StatusCode.NotFound);
            }
            
            var comments = await _commentRepository.GetAllById(article.ArticleId);

            if (comments != null!)
            {
                await _commentRepository.DeleteAllAsync(comments);
            }
            

            await _articleRepository.DeleteAsync(article);

            return new BaseResponse<ArticleEntity>().ServerResponse("Task deleted successfully!", StatusCode.NoContent);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().ServerResponse("Internal server error!", StatusCode.InternalServerError);
        }
    }
}