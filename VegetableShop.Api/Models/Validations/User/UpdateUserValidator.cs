using FluentValidation;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Models.Validations.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
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

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName required")
                .Length(2, 20)
                .WithMessage("FirstName must greater than 2 characters and less than 20 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName required")
                .Length(2, 20)
                .WithMessage("LastName must greater than 2 characters and less than 20 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("PhoneNumber required")
                .Matches("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})")
                .WithMessage("PhoneNumber invalid");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address required")
                .MaximumLength(100)
                .WithMessage("Address must less than 100 characters");
        }
    }
}

