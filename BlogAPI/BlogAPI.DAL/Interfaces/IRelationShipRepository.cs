using System.Threading.Tasks;

namespace BlogAPI.DAL.Interfaces;

public interface IRelationShipRepository<T> where T : class
{
    Task AddRelationShipAsync(T entity);
}