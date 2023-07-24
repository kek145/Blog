using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.DAL.Interfaces;

public interface IBaseRepository<T>
{
    Task AddAsync(T entity);
    IQueryable<T> GetAll();
    Task DeleteAsync(T entity);
    Task<T> UpdateAsync(T entity);
}