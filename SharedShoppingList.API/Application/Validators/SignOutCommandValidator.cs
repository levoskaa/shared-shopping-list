using FluentValidation;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Infrastructure.ErrorHandling;

namespace SharedShoppingList.API.Application.Validators
{
    public class SignOutCommandValidator : AbstractValidator<SignOutCommand>
    {
        public SignOutCommandValidator()
        {
            RuleFor(e => e.UserId)
                .NotEmpty().WithMessage(ValidationErrors.UserIdRequired);

            RuleFor(e => e.RefreshToken)
                .NotEmpty().WithMessage(ValidationErrors.RefreshTokenRequired);
        }
    }
}