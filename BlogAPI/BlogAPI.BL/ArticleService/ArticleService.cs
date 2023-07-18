using System;
using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using BlogAPI.DAL.ArticleRepository;
using BlogAPI.DAL.CategoryRepository;
using BlogAPI.Domain.Entity.Connection;
using BlogAPI.DAL.UserArticleRepository;
using BlogAPI.DAL.ArticleCategoryRepository;

namespace BlogAPI.BL.ArticleService;

public class ArticleService : IArticleService
{
    private readonly ILogger<ArticleService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IArticleRepository _articleRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserArticleRepository _userArticleRepository;
    private readonly IArticleCategoryRepository _articleCategoryRepository;

    public ArticleService(ILogger<ArticleService> logger,
        IJwtTokenService jwtTokenService,
        IArticleRepository articleRepository,
        ICategoryRepository categoryRepository,
        IUserArticleRepository userArticleRepository,
        IArticleCategoryRepository articleCategoryRepository)
    {
        _logger = logger;
        _jwtTokenService = jwtTokenService;
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
        _userArticleRepository = userArticleRepository;
        _articleCategoryRepository = articleCategoryRepository;
    }

    public async Task<IBaseResponse<ArticleEntity>> DeleteArticleAsync(string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.FindArticleByIdAsync(articleId);
            if (userId.HasValue)
            {
                if (article == null!)
                {
                    _logger.LogError("Article not found!");
                    return new BaseResponse<ArticleEntity>().BadRequestResponse("Article not found!");
                }

                await _articleRepository.DeleteArticleAsync(article);
            }

            return new BaseResponse<ArticleEntity>().SuccessRequest("Task deleted successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().InternalServerErrorResponse("Internal server error");
        }
    }

    public async Task<IBaseResponse<ArticleEntity>> CreateNewArticleAsync(ArticleDtoCreate articleDto, string token)
    {
        var userId = _jwtTokenService.GetUserIdFromToken(token);
        var category = await _categoryRepository.FindCategoryByNameAsync(articleDto.CategoryName);
        var title = await _articleRepository.FindArticleByTitleAsync(articleDto.Title);
        try
        {
            if (userId.HasValue)
            {
                if (category == null!)
                {
                    _logger.LogError("This category does not exist");
                    return new BaseResponse<ArticleEntity>().BadRequestResponse("This category does not exist.");
                }

                if (title != null!)
                {
                    _logger.LogError("There is already an article with the same title");
                    return new BaseResponse<ArticleEntity>().BadRequestResponse("There is already an article with the same title.");
                }

                var article = new ArticleEntity
                {
                    Title = articleDto.Title,
                    Content = articleDto.Content,
                    CreatedAt = articleDto.CreatedAt
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

                await _articleRepository.AddArticleAsync(article);
                await _userArticleRepository.AddUserArticleAsync(userArticle);
                await _articleCategoryRepository.AddArticleCategoryAsync(articleCategory);
            }

            _logger.LogInformation("The article has been successfully created!");
            return new BaseResponse<ArticleEntity>().SuccessRequest("The article has been successfully created!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().InternalServerErrorResponse("Internal server error");
        }
    }

    public async Task<IBaseResponse<ArticleEntity>> UpdateArticleAsync(ArticleDtoUpdate articleDto, string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.FindArticleByIdAsync(articleId);
            var title = await _articleRepository.FindArticleByTitleAsync(articleDto.Title);

            if (userId.HasValue)
            {
                if (article == null!)
                {
                    _logger.LogError("Article not found!");
                    return new BaseResponse<ArticleEntity>().BadRequestResponse("Article not found!");
                }

                if (title != null!)
                {
                    _logger.LogError("There is already an article with the same title!");
                    return new BaseResponse<ArticleEntity>().BadRequestResponse("There is already an article with the same title.");
                }

                article.Title = articleDto.Title;
                article.Content = articleDto.Content;
                article.UpdatedAt = articleDto.UpdatedAt;

                await _articleRepository.UpdateArticleAsync(article);
            }
            else throw new UnauthorizedAccessException("User is not authorized to update this article");

            _logger.LogInformation("Article successfully updated!");
            return new BaseResponse<ArticleEntity>().SuccessRequest("Article successfully updated!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().InternalServerErrorResponse("Internal server error");
        }
    }
}