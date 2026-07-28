using Catalog.Domain.Models;
using System.Linq.Expressions;

namespace Catalog.Domain.Interfaces
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        Task Add(T entity);

        Task<T?> Get(int id, params Expression<Func<T, object>>[] includes);

        Task<List<T>> All(params Expression<Func<T, object>>[] includes);

        Task<int> SaveChanges();
    }
}