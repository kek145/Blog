using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogAPI.DAL.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllById(int commentId);
    Task<List<T>> GetAllByUserId(int userId);
    Task<List<T>> DeleteAllAsync(IEnumerable<T> entity);
}