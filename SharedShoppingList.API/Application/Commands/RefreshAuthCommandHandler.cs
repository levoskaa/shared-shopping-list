using MediatR;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Application.Commands
{
    public class RefreshAuthCommandHandler : IRequestHandler<RefreshAuthCommand, AuthenticationResult>
    {
        private readonly IRepository<User> userRepository;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;

        public RefreshAuthCommandHandler(
            IRepository<User> userRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuthenticationResult> Handle(RefreshAuthCommand command, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredAccessToken(command.AccessToken);
            var userId = principal.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)
                ?.Value;
            var user = await userRepository.GetByIdAsync(
                userId,
                cancellationToken,
                nameof(User.RefreshTokens));

            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            if (!user.VerifyRefreshToken(command.RefreshToken))
            {
                throw new UnauthorizedException("Refresh token is invalid",
                    ValidationErrors.RefreshTokenInvalid);
            }

            var newAccessToken = await tokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.RemoveRefreshToken(command.RefreshToken);
            user.AddRefreshToken(newRefreshToken);
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken.Value,
                AccessTokenExpiryTime = newAccessToken.ValidTo,
            };
        }
    }
}