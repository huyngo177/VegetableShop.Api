using FluentValidation;
using VegetableShop.Api.Dto.Category;

namespace VegetableShop.Api.Models.Validations.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name required")
                .Length(2, 20)
                .WithMessage("Name must greater than 2 characters and less than 20 characters");
        }
    }
}
