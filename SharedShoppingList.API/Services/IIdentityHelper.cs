using System.Security.Claims;

namespace SharedShoppingList.API.Services
{
    public interface IIdentityHelper
    {
        ClaimsPrincipal? ClaimsPrincipal { get; }
        string? GetAuthenticatedUserId();
        string? GetAuthenticatedUsername();
    }
}