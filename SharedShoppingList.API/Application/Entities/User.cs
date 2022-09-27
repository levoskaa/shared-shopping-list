using Microsoft.AspNetCore.Identity;

namespace SharedShoppingList.API.Application.Entities
{
    public class User : IdentityUser
    {
        public IEnumerable<RefreshToken> RefreshTokens { get; set; } = Enumerable.Empty<RefreshToken>();
    }
}
