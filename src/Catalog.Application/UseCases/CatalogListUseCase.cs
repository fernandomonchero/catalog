using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappings;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.UseCases
{
    public class CatalogListUseCase : ICatalogListUseCase
    {
        private readonly IProductRepository _productRepository;

        public CatalogListUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> List()
        {
            var products = await _productRepository.All(p => p.Sellers);

            return ProductMapper.ToDtoSet(products);
        }
    }
}