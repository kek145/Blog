using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.DAL.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task AddAsync(T entity);
    IQueryable<T> GetAll();
    Task DeleteAsync(T entity);
    Task<T> UpdateAsync(T entity);
}