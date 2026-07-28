using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class SellerProductRepository : Repository<SellerProduct>, ISellerProductRepository
    {
        public SellerProductRepository(CatalogContext context) : base(context)
        {
        }

        public async Task<bool> ProductAlreadyHasTheSeller(int productId, string sellerName)
        {
            return await EntitySet.AnyAsync(sp => sp.ProductId == productId && sp.SellerName == sellerName);
        }
    }
}