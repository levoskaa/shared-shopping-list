using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;

        public SignInCommandHandler(
            UserManager<User> userManager,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuthenticationResult> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(command.Username);
            if (user == null || !(await userManager.CheckPasswordAsync(user, command.Password)))
            {
                throw new DomainException(
                    "Invalid sign in credentials",
                    ValidationErrors.SignInCredentialsInvalid);
            }

            var accessToken = await tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.AddRefreshToken(refreshToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Value,
                AccessTokenExpiryTime = accessToken.ValidTo,
            };
        }
    }
}