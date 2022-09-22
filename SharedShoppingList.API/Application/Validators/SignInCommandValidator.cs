using FluentValidation;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Infrastructure.ErrorHandling;

namespace SharedShoppingList.API.Application.Validators
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(e => e.Username).NotEmpty()
                .WithMessage(ValidationErrors.UsernameRequired);
            RuleFor(e => e.Password).NotEmpty()
                .WithMessage(ValidationErrors.PasswordRequired);
        }
    }
}