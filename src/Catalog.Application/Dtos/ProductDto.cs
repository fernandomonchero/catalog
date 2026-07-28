using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; } = string.Empty;

        public string SellerName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;
    }
}