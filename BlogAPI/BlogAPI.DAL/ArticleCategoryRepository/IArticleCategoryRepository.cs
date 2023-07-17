using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.ArticleCategoryRepository;

public interface IArticleCategoryRepository
{
    Task CreateArticleCategoryAsync(ArticleCategoryEntity entity);
    Task DeleteArticleCategoryAsync(ArticleCategoryEntity entity);
    Task<ArticleCategoryEntity> FindArticleCategoryByIdAsync(int articleId, int categoryId);
}