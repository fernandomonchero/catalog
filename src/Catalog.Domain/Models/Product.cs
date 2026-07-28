using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Domain.Models
{
    [Table("Product")]
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public ICollection<SellerProduct> Sellers { get; set; } = [];

        [NotMapped]
        public string SellerProductId { get; set; } = string.Empty;
    }
}