using FluentValidation;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Infrastructure.ErrorHandling;

namespace SharedShoppingList.API.Application.Validators
{
    public class RefreshAuthCommandValidator : AbstractValidator<RefreshAuthCommand>
    {
        public RefreshAuthCommandValidator()
        {
            RuleFor(e => e.AccessToken)
                .NotEmpty().WithErrorCode(ValidationErrors.UserIdRequired);
            RuleFor(e => e.RefreshToken)
                .NotEmpty().WithErrorCode(ValidationErrors.RefreshTokenRequired);
        }
    }
}