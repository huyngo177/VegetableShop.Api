using FluentValidation;
using VegetableShop.Api.Dto.Orders;

namespace VegetableShop.Api.Models.Validations.Order
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderValidator()
        {
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
