using FluentValidation;
using SharedShoppingList.API.Application.Commands.UserCommands;
using SharedShoppingList.API.Infrastructure.ErrorHandling;

namespace SharedShoppingList.API.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(e => e.Username)
                .NotEmpty().WithErrorCode(ValidationErrors.UsernameRequired);
            RuleFor(e => e.Password)
                .NotEmpty().WithErrorCode(ValidationErrors.PasswordRequired)
                .MinimumLength(8).WithErrorCode(ValidationErrors.MinPasswordLength)
                // (?=.*\d) - at least one digit exists
                .Matches(@"(?=.*\d)").WithErrorCode(ValidationErrors.PasswordDigitRequired)
                // (?=.*[a-z]) - at least one lowercase letter exists
                .Matches(@"(?=.*[a-z])").WithErrorCode(ValidationErrors.PasswordLowercaseRequired)
                // (?=.*[A-Z]) - at least one uppercase letter exists
                .Matches(@"(?=.*[A-Z])").WithErrorCode(ValidationErrors.PasswordUppercaseRequired);
        }
    }
}