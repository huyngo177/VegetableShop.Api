using FluentValidation;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Models.Validations.User
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
               .NotEmpty()
               .WithMessage("Password required")
               .NotNull()
               .WithMessage("Password required")
               .Length(6, 20)
               .WithMessage("Password must greater than 6 characters and less than 20 characters");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New Password required")
                .NotNull()
                .WithMessage("New Password required")
                .Length(6, 20)
                .WithMessage("New Password must greater than 6 characters and less than 20 characters");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Confirm Password do not match");
        }
    }
}
