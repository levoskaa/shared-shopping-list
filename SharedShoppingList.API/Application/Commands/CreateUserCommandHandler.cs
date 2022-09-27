using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Application.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenGenerator tokenGenerator;

        public CreateUserCommandHandler(
            UserManager<User> userManager,
            ITokenGenerator tokenGenerator)
        {
            this.userManager = userManager;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<AuthenticationResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByNameAsync(command.Username);
            if (existingUser != null)
            {
                throw new DomainException("Username already taken",
                    ValidationErrors.UsernameTaken);
            }

            var user = new User
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = command.Username
            };
            var userCreationResult = await userManager.CreateAsync(user, command.Password);
            if (!userCreationResult.Succeeded)
            {
                throw new Exception(); // TODO: use custom exception for 500
            }

            var accessToken = await tokenGenerator.GenerateAccessTokenAsync(user);
            var refreshToken = tokenGenerator.GenerateRefreshToken();

            user.AddRefreshToken(refreshToken);
            await userManager.UpdateAsync(user);

            return new AuthenticationResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                AccessTokenExpiryTime = accessToken.ValidTo,
            };
        }
    }
}