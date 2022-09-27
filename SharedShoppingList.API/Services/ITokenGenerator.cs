using SharedShoppingList.API.Application.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SharedShoppingList.API.Services
{
    public interface ITokenGenerator
    {
        Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken();
    }
}