using FluentValidation;
using VegetableShop.Api.Dto.Products;

namespace VegetableShop.Api.Models.Validations.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.Name)
              .NotEmpty()
              .WithMessage("Product required")
              .Length(2, 20)
              .WithMessage("Name must greater than 2 characters and less than 20 characters");
            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                .WithMessage("Price required")
                .GreaterThan(0)
                .WithMessage("Price must greater than 0");
            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("Price required")
                .GreaterThan(0)
                .WithMessage("Stock must greater than 0");
        }
    }
}
