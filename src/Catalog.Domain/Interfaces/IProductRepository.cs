using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByNameAndBrand(string name, string brand);
    }
}