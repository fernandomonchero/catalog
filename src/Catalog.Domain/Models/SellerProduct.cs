using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Domain.Models
{
    [Table("SellerProduct")]
    public class SellerProduct : Entity
    {        
        public string SellerName { get; set; } = string.Empty;
        public string SellerProductId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}