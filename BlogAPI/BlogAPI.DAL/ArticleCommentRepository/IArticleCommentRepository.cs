using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.ArticleCommentRepository;

public interface IArticleCommentRepository
{
    Task AddArticleCommentAsync(ArticleCommentEntity entity);
}