using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Infrastructure.ErrorHandling;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                    $"Command validation errors for type {typeof(SignInCommand).Name}",
                    ValidationErrors.SignInCredentialsInvalid);
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = tokenGenerator.GenerateToken(claims);
            return new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpirationTime = token.ValidTo,
            };
        }
    }
}