using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenGenerator tokenGenerator;

        public SignInCommandHandler(
            UserManager<User> userManager,
            ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.tokenGenerator = tokenGenerator;
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

            var accessToken = await tokenGenerator.GenerateAccessTokenAsync(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();
            // TODO: save refresh token
            return new AuthenticationResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                AccessTokenExpiryTime = accessToken.ValidTo,
            };
        }
    }
}