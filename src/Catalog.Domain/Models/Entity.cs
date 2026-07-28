using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Models
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}