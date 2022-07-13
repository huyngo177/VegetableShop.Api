using FluentValidation;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Models.Validations.User
{
    public class CreateAppUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateAppUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email required")
                .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
                .WithMessage("Email invalid");
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Username required")
                .Length(5, 20)
                .WithMessage("Username must greater than 5 characters and less than 20 characters");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password required")
                .NotNull()
                .WithMessage("Password required")
                .Length(6, 20)
                .WithMessage("Password must greater than 6 characters and less than 20 characters");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Firstname required")
                .Length(2, 20)
                .WithMessage("Username must greater than 2 characters and less than 20 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Lastname required")
                .Length(2, 20)
                .WithMessage("Username must greater than 2 characters and less than 20 characters");

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
