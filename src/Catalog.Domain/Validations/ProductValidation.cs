using Catalog.Domain.Models;
using FluentValidation;

namespace Catalog.Domain.Validations
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage(p => $"{p.SellerProductId} - Name is required");
        }
    }
}