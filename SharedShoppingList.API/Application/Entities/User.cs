using Microsoft.AspNetCore.Identity;

namespace SharedShoppingList.API.Application.Entities
{
    public class User : IdentityUser
    {
        public IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Add(refreshToken);
        }
    }
}