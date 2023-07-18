using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.ArticleCategoryRepository;

public interface IArticleCategoryRepository
{
    IQueryable<ArticleCategoryEntity> GetAllArticleCategories();
    Task AddArticleCategoryAsync(ArticleCategoryEntity entity);
    Task<ArticleCategoryEntity> FindArticleCategoryByIdAsync(int articleId, int categoryId);
}