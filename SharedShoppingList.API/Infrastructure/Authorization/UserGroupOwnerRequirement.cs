using Microsoft.AspNetCore.Authorization;

namespace SharedShoppingList.API.Infrastructure.Authorization
{
    public class UserGroupOwnerRequirement : IAuthorizationRequirement
    {
    }
}