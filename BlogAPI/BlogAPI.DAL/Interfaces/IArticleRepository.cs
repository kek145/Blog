using System.Collections.Generic;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Interfaces;

public interface IArticleRepository
{
    Task<List<ArticleEntity>> GetAllById(int articleId);
    Task<List<ArticleEntity>> GetAllByUserId(int userId);
    Task<List<ArticleEntity>> DeleteAllAsync(IEnumerable<ArticleEntity> entity);
}