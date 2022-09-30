using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Application.Commands
{
    public class RefreshAuthCommandHandler : IRequestHandler<RefreshAuthCommand, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;

        public RefreshAuthCommandHandler(
            UserManager<User> userManager,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Handle(RefreshAuthCommand command, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredAccessToken(command.AccessToken);
            var userId = principal.Claims
                .FirstOrDefault(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub))
                ?.Value;
            var user = await userManager.Users
                .Include(user => user.RefreshTokens)
                .SingleAsync(user => user.Id == userId, cancellationToken);

            if (!user.VerifyRefreshToken(command.RefreshToken))
            {
                throw new UnauthorizedException("Refresh token is invalid",
                    ValidationErrors.RefreshTokenInvalid);
            }

            var newAccessToken = await tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.RemoveRefreshToken(command.RefreshToken);
            user.AddRefreshToken(newRefreshToken);
            await userManager.UpdateAsync(user);

            return new AuthenticationResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken.Value,
                AccessTokenExpiryTime = newAccessToken.ValidTo,
            };
        }
    }
}