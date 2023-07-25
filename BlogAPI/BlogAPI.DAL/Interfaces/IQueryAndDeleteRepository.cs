using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlogAPI.DAL.Interfaces;

public interface IQueryAndDeleteRepository<T> where T : class
{
    Task<List<T>> GetAllById(int commentId);
    Task<List<T>> GetAllByUserId(int userId);
    Task<List<T>> DeleteAllAsync(IEnumerable<T> entity);
}