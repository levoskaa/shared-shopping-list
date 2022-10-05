using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SharedShoppingList.API.Services
{
    public class IdentityHelper : IIdentityHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClaimsPrincipal? ClaimsPrincipal => httpContextAccessor.HttpContext?.User;

        public IdentityHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string? GetAuthenticatedUserId()
        {
            return httpContextAccessor?.HttpContext?.User
                .FindFirstValue(JwtRegisteredClaimNames.Sub);
        }

        public string? GetAuthenticatedUsername()
        {
            return httpContextAccessor?.HttpContext?.User
                .FindFirstValue(ClaimTypes.Name);
        }
    }
}