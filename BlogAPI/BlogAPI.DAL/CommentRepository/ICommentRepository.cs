using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.CommentRepository;

public interface ICommentRepository
{
    IQueryable<CommentEntity> GetAllComments();
    Task AddCommentAsync(CommentEntity entity);
    Task DeleteCommentAsync(CommentEntity entity);
    Task<CommentEntity> UpdateCommentAsync(CommentEntity entity);
}