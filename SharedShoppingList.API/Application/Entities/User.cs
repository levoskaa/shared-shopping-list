using Microsoft.AspNetCore.Identity;

namespace SharedShoppingList.API.Application.Entities
{
    public class User : IdentityUser
    {
        private readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens;

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            refreshTokens.Add(refreshToken);
        }

        public void RevokeRefreshToken(RefreshToken refreshToken)
        {
            refreshTokens.Remove(refreshToken);
        }

        public void RevokeAllRefreshTokens()
        {
            refreshTokens.Clear();
        }
    }
}