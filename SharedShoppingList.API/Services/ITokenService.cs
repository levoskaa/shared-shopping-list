using SharedShoppingList.API.Application.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SharedShoppingList.API.Services
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken);
    }
}