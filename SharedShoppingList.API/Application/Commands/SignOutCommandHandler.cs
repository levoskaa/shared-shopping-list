using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
    {
        private readonly UserManager<User> userManager;

        public SignOutCommandHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(SignOutCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
                .Include(user => user.RefreshTokens)
                .SingleAsync(user => user.Id == command.UserId, cancellationToken);
            var refreshTokenToRemove = user.RefreshTokens
                .SingleOrDefault(token => token.Token == command.RefreshToken);
            if (refreshTokenToRemove != null)
            {
                user.RevokeRefreshToken(refreshTokenToRemove);
                await userManager.UpdateAsync(user);
            }
            return Unit.Value;
        }
    }
}