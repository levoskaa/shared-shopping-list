using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Token>
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

        public async Task<Token> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByNameAsync(command.Username);
            if (user == null || !(await userManager.CheckPasswordAsync(user, command.Password)))
            {
                throw new DomainException(
                    "Invalid sign in credentials",
                    ValidationErrors.SignInCredentialsInvalid);
            }

            var token = await tokenGenerator.GenerateTokenAsync(user);
            return new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationTime = token.ValidTo,
            };
        }
    }
}