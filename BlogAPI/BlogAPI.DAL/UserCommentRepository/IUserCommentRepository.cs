using System.Threading.Tasks;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.DAL.UserCommentRepository;

public interface IUserCommentRepository
{
    Task AddUserCommentAsync(UserCommentEntity entity);
}