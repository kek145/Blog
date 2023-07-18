using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.ArticleCategoryRepository;

public interface IArticleCategoryRepository
{
    Task AddArticleCategoryAsync(ArticleCategoryEntity entity);
    Task DeleteArticleCategoryAsync(ArticleCategoryEntity entity);
    Task<ArticleCategoryEntity> FindArticleCategoryByIdAsync(int articleId, int categoryId);
}