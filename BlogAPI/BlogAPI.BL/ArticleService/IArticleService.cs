using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using System.Collections.Generic;
using BlogAPI.BL.DTOs.ArticleDTOs;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.BL.ArticleService;

public interface IArticleService
{
    Task<IBaseResponse<IEnumerable<ArticleDto>>> GetAllArticles();
    Task<IBaseResponse<ArticleEntity>> DeleteArticleAsync(string token, int articleId);
    Task<IBaseResponse<ArticleEntity>> CreateNewArticleAsync(ArticleCreateDto articleDto, string token);
    Task<IBaseResponse<ArticleEntity>> UpdateArticleAsync(ArticleUpdateDto articleDto, string token, int articleId);
}