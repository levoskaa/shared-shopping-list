using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Application.Commands.ShoppingListEntryCommands
{
    public class GetShoppingListEntriesCommandHandler
        : IRequestHandler<GetShoppingListEntriesCommand, PaginatedList<ShoppingListEntry>>
    {
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IIdentityHelper identityHelper;

        public GetShoppingListEntriesCommandHandler(
            IRepository<UserGroup> userGroupRepository,
            IAuthorizationService authorizationService,
            IIdentityHelper identityHelper)
        {
            this.userGroupRepository = userGroupRepository;
            this.authorizationService = authorizationService;
            this.identityHelper = identityHelper;
        }

        public async Task<PaginatedList<ShoppingListEntry>> Handle(GetShoppingListEntriesCommand command, CancellationToken cancellationToken)
        {
            var userGroup = await userGroupRepository.GetByIdAsync(
                command.GroupId,
                cancellationToken,
                nameof(UserGroup.ShoppingListEntries));
            if (userGroup == null)
            {
                throw new EntityNotFoundException("UserGroup not found");
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(
                identityHelper.ClaimsPrincipal,
                userGroup,
                new UserGroupMemberRequirement());
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            return new PaginatedList<ShoppingListEntry>(
                userGroup.ShoppingListEntries,
                userGroup.ShoppingListEntries.Count,
                command.PageSize);
        }
    }
}