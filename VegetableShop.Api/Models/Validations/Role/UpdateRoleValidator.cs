using FluentValidation;
using VegetableShop.Api.Dto.Role;

namespace VegetableShop.Api.Models.Validations.Role
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name required")
                .Length(6, 20)
                .WithMessage("Name must greater than 6 characters and less than 20 characters");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description required")
                .Length(2, 255)
                .WithMessage("Name must greater than 2 characters and less than 255 characters");
        }
    }
}
