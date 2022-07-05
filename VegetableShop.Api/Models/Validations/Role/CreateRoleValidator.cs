using FluentValidation;
using VegetableShop.Api.Dto.Role;

namespace VegetableShop.Api.Models.Validations.Role
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name required")
                .Length(6, 20)
                .WithMessage("Name must greater than 6 characters and less than 20 characters");
            RuleFor(x => x.Description)
                .MaximumLength(200)
                .WithMessage("Description must less than 200 characters");
        }
    }
}
