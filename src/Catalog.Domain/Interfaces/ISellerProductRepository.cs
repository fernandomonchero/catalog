using Catalog.Domain.Models;

namespace Catalog.Domain.Interfaces
{
    public interface ISellerProductRepository : IRepository<SellerProduct>
    {
        Task<bool> ProductAlreadyHasTheSeller(int productId, string sellerName);
    }
}