using FluentValidation;
using VegetableShop.Api.Dto.Products;

namespace VegetableShop.Api.Models.Validations.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Product required")
                .Length(6, 20)
                .WithMessage("Name must greater than 6 characters and less than 20 characters");
            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                .WithMessage("Price required");
            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                .WithMessage("Price required");
            RuleFor(p => p.ImagePath)
                .MaximumLength(200)
                .WithMessage("Image path must less than 200 characters");
            RuleFor(p => p.CategoryName)
                .MaximumLength(20)
                .WithMessage("Category Name must less than 20 characters");
        }
    }
}
