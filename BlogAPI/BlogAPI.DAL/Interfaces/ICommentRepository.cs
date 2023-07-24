using System.Threading.Tasks;
using System.Collections.Generic;
using BlogAPI.Domain.Entity.Table;

namespace BlogAPI.DAL.Interfaces;

public interface ICommentRepository
{
    Task<List<CommentEntity>> GetAllById(int commentId);
    Task<List<CommentEntity>> GetAllByUserId(int userId);
    Task<List<CommentEntity>> DeleteAllAsync(IEnumerable<CommentEntity> entity);
}