using Catalog.Domain.Extensions;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Models;
using Catalog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogContext context) : base(context)
        {
        }

        public async Task<Product?> GetByNameAndBrand(string name, string brand)
        {
            var cleanName = name.NormalizeSpaces();
            var cleanBrand = string.IsNullOrWhiteSpace(brand) ? null : brand.NormalizeSpaces();

            if (string.IsNullOrWhiteSpace(cleanName))
                return null;
                
            return await EntitySet.FirstOrDefaultAsync(p =>
                p.Name.ToLower() == cleanName.ToLower() &&
                    (cleanBrand == null || p.Brand.ToLower() == cleanBrand.ToLower())
            );
        }
    }
}