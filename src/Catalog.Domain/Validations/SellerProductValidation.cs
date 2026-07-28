using Catalog.Domain.Models;
using FluentValidation;

namespace Catalog.Domain.Validations
{
    public class SellerProductValidation : AbstractValidator<SellerProduct>
    {
        public SellerProductValidation()
        {
            RuleFor(s => s.SellerName)
                .NotEmpty()
                .WithMessage(s => $"{s.SellerProductId} - SellerName is required");
            RuleFor(s => s.SellerProductId)
                .NotEmpty()
                .WithMessage(s => $"{s.SellerProductId} - SellerProductId is required");
        }
    }
}