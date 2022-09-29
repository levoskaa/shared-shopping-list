using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class RevokeAllRefreshTokensCommandHandler : IRequestHandler<RevokeAllRefreshTokensCommand>
    {
        private readonly UserManager<User> userManager;

        public RevokeAllRefreshTokensCommandHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(RevokeAllRefreshTokensCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
                .Include(user => user.RefreshTokens)
                .SingleAsync(user => user.Id == command.UserId, cancellationToken);
            user.RevokeAllRefreshTokens();
            await userManager.UpdateAsync(user);
            return Unit.Value;
        }
    }
}