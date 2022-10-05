using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Infrastructure.Authorization
{
    public class UserGroupOwnerRequirementHandler
        : AuthorizationHandler<UserGroupOwnerRequirement, UserGroup>
    {
        private readonly IIdentityHelper identityHelper;

        public UserGroupOwnerRequirementHandler(IIdentityHelper identityHelper)
        {
            this.identityHelper = identityHelper;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserGroupOwnerRequirement requirement,
            UserGroup userGroup)
        {
            bool isUserOwnerOfGroup = userGroup.OwnerId == identityHelper.GetAuthenticatedUserId();
            if (isUserOwnerOfGroup)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}