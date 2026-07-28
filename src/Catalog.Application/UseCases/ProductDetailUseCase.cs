using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappings;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.UseCases
{
    public class ProductDetailUseCase : IProductDetailUseCase
    {
        private readonly IProductRepository _productRepository;

        public ProductDetailUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Detail(int id)
        {
            var product = await _productRepository.Get(id);

            if (product == null) return null;

            return ProductMapper.ToDto(product);
        }
    }
}
