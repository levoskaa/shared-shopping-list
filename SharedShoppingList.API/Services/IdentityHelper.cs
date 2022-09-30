using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SharedShoppingList.API.Services
{
    public class IdentityHelper : IIdentityHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public IdentityHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetAuthenticatedUserId()
        {
            return httpContextAccessor?.HttpContext?.User
                .FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "";
        }
    }
}