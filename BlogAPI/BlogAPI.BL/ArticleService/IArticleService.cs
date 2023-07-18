using BlogAPI.BL.DTOs;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.BL.ArticleService;

public interface IArticleService
{
    Task<IBaseResponse<ArticleEntity>> CreateNewArticleAsync(ArticleDtoCreate articleDto, string token);
    Task<IBaseResponse<ArticleEntity>> UpdateArticleAsync(ArticleDtoUpdate articleDto, string token, int articleId);
}