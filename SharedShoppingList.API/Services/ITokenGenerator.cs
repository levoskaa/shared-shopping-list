using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SharedShoppingList.API.Services
{
    public interface ITokenGenerator
    {
        JwtSecurityToken GenerateToken(IEnumerable<Claim> claims);
    }
}