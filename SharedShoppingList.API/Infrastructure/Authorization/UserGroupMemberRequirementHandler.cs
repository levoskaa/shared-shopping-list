using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Infrastructure.Authorization
{
    public class UserGroupMemberRequirementHandler
        : AuthorizationHandler<UserGroupMemberRequirement, UserGroup>
    {
        private readonly IRepository<User> userRepository;
        private readonly IIdentityHelper identityHelper;

        public UserGroupMemberRequirementHandler(
            IRepository<User> userRepository,
            IIdentityHelper identityHelper)
        {
            this.userRepository = userRepository;
            this.identityHelper = identityHelper;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserGroupMemberRequirement requirement,
            UserGroup userGroup)
        {
            var userId = identityHelper.GetAuthenticatedUserId();
            var user = await userRepository.GetByIdAsync(userId, default, nameof(User.Groups));
            if (user.IsMemberOfGroup(userGroup))
            {
                context.Succeed(requirement);
            }
        }
    }
}