using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.ArticleRepository;

public interface IArticleRepository
{
    IQueryable<ArticleEntity> GetAllArticles();
    Task AddArticleAsync(ArticleEntity entity);
    Task UpdateArticleAsync(ArticleEntity entity);
    Task DeleteArticleAsync(ArticleEntity entity);
    Task<ArticleEntity> FindArticleByIdAsync(int articleId);
    Task<ArticleEntity> FindArticleByTitleAsync(string title);
}