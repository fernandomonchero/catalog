using Catalog.Application.Dtos;
using Catalog.Domain.Models;

namespace Catalog.Application.Mappings
{
    public static class ProductMapper
    {
        public static List<Product> ToEntitySet(List<ProductDto> productDtos)
        {
            return productDtos.Select(ToEntity).ToList();
        }

        public static Product ToEntity(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                SellerProductId = productDto.Id,
                Sellers = new List<SellerProduct>()
            };

            if (!string.IsNullOrWhiteSpace(productDto.SellerName))
            {
                product.Sellers.Add(new SellerProduct
                {
                    SellerName = productDto.SellerName,
                    SellerProductId = productDto.Id
                });
            }

            return product;
        }

        public static List<ProductDto> ToDtoSet(List<Product> products)
        {
            return products.Select(ToDto).ToList();
        }

        public static ProductDto ToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                SellerName = string.Join(", ", product.Sellers.Select(s => s.SellerName)) ?? string.Empty
            };
        }   
    }
}