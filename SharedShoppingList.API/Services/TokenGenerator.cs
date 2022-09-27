using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedShoppingList.API.Application.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharedShoppingList.API.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public TokenGenerator(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateTokenAsync(User user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var claims = await GetUserClaimsAsync(user);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(double.Parse(configuration["JWT:AccessTokenLifespanInHours"])),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        private async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
        {
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

            return claims;
        }
    }
}