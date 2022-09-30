using Microsoft.AspNetCore.Identity;

namespace SharedShoppingList.API.Application.Entities
{
    public class User : IdentityUser
    {
        private readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();
        public virtual IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens;

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            refreshTokens.Add(refreshToken);
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            var refreshTokenEntity = refreshTokens.SingleOrDefault(token => token.Value == refreshToken);
            if (refreshTokenEntity != null)
            {
                RemoveRefreshToken(refreshTokenEntity);
            }
        }

        public void RemoveRefreshToken(RefreshToken refreshToken)
        {
            refreshTokens.Remove(refreshToken);
        }

        public void RemoveAllRefreshTokens()
        {
            refreshTokens.Clear();
        }

        public bool VerifyRefreshToken(string refreshToken)
        {
            var refreshTokenEntity = refreshTokens.SingleOrDefault(token => token.Value == refreshToken);
            if (refreshTokenEntity == null)
            {
                return false;
            }
            bool expired = refreshTokenEntity.ExpiryTime < DateTime.UtcNow;
            if (expired)
            {
                return false;
            }
            return true;
        }
    }
}