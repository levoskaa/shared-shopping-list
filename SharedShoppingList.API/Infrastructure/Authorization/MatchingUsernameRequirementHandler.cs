using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SharedShoppingList.API.Infrastructure.Authorization
{
    public class MatchingUsernameRequirementHandler : AuthorizationHandler<MatchingUsernameRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MatchingUsernameRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MatchingUsernameRequirement requirement)
        {
            var requestedUsername = httpContextAccessor.HttpContext
                .GetRouteData()
                .Values["username"] as string;
            bool claimMatchesRoute = context.User.HasClaim(claim =>
                claim.Type == ClaimTypes.Name && claim.Value == requestedUsername);
            if (claimMatchesRoute)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
