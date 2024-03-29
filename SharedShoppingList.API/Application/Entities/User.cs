﻿using Microsoft.AspNetCore.Identity;

namespace SharedShoppingList.API.Application.Entities
{
    public class User : IdentityUser
    {
        private readonly List<RefreshToken> refreshTokens = new();
        public virtual IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens;

        private readonly List<UserUserGroup> userUserGroups = new();
        public virtual IReadOnlyCollection<UserUserGroup> UserUserGroups => userUserGroups;

        private readonly List<UserGroup> groups = new();
        public virtual IReadOnlyCollection<UserGroup> Groups => groups;

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

        public void AddUserGroup(UserGroup group)
        {
            groups.Add(group);
        }

        public bool IsMemberOfGroup(UserGroup group)
        {
            return groups.Any(g => g.Id == group.Id);
        }
    }
}