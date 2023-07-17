using System;
using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.DAL.ArticleCategoryRepository;
using BlogAPI.DAL.ArticleRepository;
using BlogAPI.DAL.CategoryRepository;
using BlogAPI.DAL.UserArticleRepository;
using BlogAPI.Domain.Entity.Connection;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;
using Microsoft.Extensions.Logging;

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

    public async Task<IBaseResponse<ArticleEntity>> CreateNewArticleAsync(ArticleDtoCreate articleDto, string token)
    {
        int? userId = _jwtTokenService.GetUserIdFromToken(token);
        var article = await _articleRepository.FindArticleByTitleAsync(articleDto.Title);
        var category = await _categoryRepository.FindCategoryByNameAsync(articleDto.CategoryName);
        var userArticle = await _userArticleRepository.FindUserArticleByIdAsync(userId!.Value, article.ArticleId);
        try
        {
            if (category == null!)
            {
                _logger.LogError("This category does not exist");
                return new BaseResponse<ArticleEntity>().BadRequestResponse("This category does not exist.");
            }

            if (article != null!)
            {
                _logger.LogError("There is already an article with the same title");
                return new BaseResponse<ArticleEntity>().BadRequestResponse("There is already an article with the same title.");
            }

            if (userArticle == null!)
            {
                _logger.LogError("An error occurred while creating the article");
                return new BaseResponse<ArticleEntity>().BadRequestResponse("An error occurred while creating the article");
            }

            var articleCategory = new ArticleCategoryEntity
            {
                Article = article,
                Category = category
            };

            var userArticleEntity = new UserArticleEntity
            {
                UserId = userId.Value,
                Article = article!
            };

            var articleEntity = new ArticleEntity
            {
                Title = articleDto.Title,
                Content = articleDto.Content,
                CreatedAt = articleDto.CreatedAt
            };

            await _articleCategoryRepository.CreateArticleCategoryAsync(articleCategory);
            await _userArticleRepository.CreateUserArticleAsync(userArticleEntity);
            await _articleRepository.CreateArticleAsync(articleEntity);
            
            _logger.LogInformation("The article has been successfully created!");
            return new BaseResponse<ArticleEntity>().SuccessRequest("The article has been successfully created!");
        }
        catch (Exception)
        {
            return new BaseResponse<ArticleEntity>().InternalServerErrorResponse("Internal server error");
        }
    }
}