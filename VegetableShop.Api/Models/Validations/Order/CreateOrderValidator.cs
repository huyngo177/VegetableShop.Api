using FluentValidation;
using VegetableShop.Api.Dto.Orders;

namespace VegetableShop.Api.Models.Validations.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .WithMessage("Username required")
                .Length(5, 20)
                .WithMessage("Username must greater than 5 characters and less than 20 characters");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Firstname required")
                .Length(2, 20)
                .WithMessage("Firstname must greater than 2 characters and less than 20 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Lastname required")
                .Length(2, 20)
                .WithMessage("Lastname must greater than 2 characters and less than 20 characters");

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
