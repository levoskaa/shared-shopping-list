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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(
            UserManager<User> userManager,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.unitOfWork = unitOfWork;
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