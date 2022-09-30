using FluentValidation;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Infrastructure.ErrorHandling;

namespace SharedShoppingList.API.Application.Validators
{
    public class RevokeAllRefreshTokensCommandValidator : AbstractValidator<RevokeAllRefreshTokensCommand>
    {
        public RevokeAllRefreshTokensCommandValidator()
        {
            RuleFor(e => e.UserId)
                .NotEmpty().WithErrorCode(ValidationErrors.UserIdRequired);
        }
    }
}
