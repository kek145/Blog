using System;
using System.Linq;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.BL.JwtTokenService;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.ArticleDTOs;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;
using BlogAPI.DAL.ArticleRepository;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IBaseResponse<IEnumerable<ArticleDto>>> GetAllArticles()
    {
        var articles = await _articleRepository.GetAllArticles()
            .Select(x =>
                new ArticleDto
                {
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }
            ).ToListAsync();
        if (articles == null!)
            return new BaseResponse<IEnumerable<ArticleDto>>().BadRequestResponse("Articles is not found!");
        return new BaseResponse<IEnumerable<ArticleDto>>
        {
            Data = articles,
            StatusCode = StatusCode.Ok,
        };
    }

    public async Task<IBaseResponse<ArticleEntity>> DeleteArticleAsync(string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.FindArticleByIdAsync(articleId);
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");
            
            if (article == null!)
            {
                _logger.LogError("Article not found!");
                return new BaseResponse<ArticleEntity>().BadRequestResponse("Article not found!");
            }

            await _articleRepository.DeleteArticleAsync(article);

            return new BaseResponse<ArticleEntity>().SuccessRequest("Task deleted successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().InternalServerErrorResponse("Internal server error");
        }
    }

    public async Task<IBaseResponse<ArticleEntity>> CreateNewArticleAsync(ArticleCreateDto articleDto, string token)
    {
        var userId = _jwtTokenService.GetUserIdFromToken(token);
        var category = await _categoryRepository.FindCategoryByNameAsync(articleDto.CategoryName);
        var title = await _articleRepository.FindArticleByTitleAsync(articleDto.Title);
        try
        {
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");
            
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

                _logger.LogInformation("The article has been successfully created!");
            return new BaseResponse<ArticleEntity>().SuccessRequest("The article has been successfully created!");
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<ArticleEntity>().InternalServerErrorResponse("Internal server error");
        }
    }

    public async Task<IBaseResponse<ArticleEntity>> UpdateArticleAsync(ArticleUpdateDto articleDto, string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.FindArticleByIdAsync(articleId);
            var title = await _articleRepository.FindArticleByTitleAsync(articleDto.Title);

            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");
            
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