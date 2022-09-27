using FluentValidation;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Infrastructure.ErrorHandling;

namespace SharedShoppingList.API.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(e => e.Username)
                .NotEmpty().WithMessage(ValidationErrors.UsernameRequired);
            RuleFor(e => e.Password)
                .NotEmpty().WithMessage(ValidationErrors.PasswordRequired)
                .MinimumLength(8).WithMessage(ValidationErrors.MinPasswordLength)
                // (?=.*\d) - at least one digit exists
                .Matches(@"(?=.*\d)").WithMessage(ValidationErrors.PasswordDigitRequired)
                // (?=.*[a-z]) - at least one lowercase letter exists
                .Matches(@"(?=.*[a-z])").WithMessage(ValidationErrors.PasswordLowercaseRequired)
                // (?=.*[A-Z]) - at least one uppercase letter exists
                .Matches(@"(?=.*[A-Z])").WithMessage(ValidationErrors.PasswordUppercaseRequired);
        }
    }
}