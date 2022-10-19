using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Application.Commands.ShoppingListEntryCommands
{
    public class UpdateShoppingListEntryCommandHandler
        : IRequestHandler<UpdateShoppingListEntryCommand, ShoppingListEntry>
    {
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IIdentityHelper identityHelper;
        private readonly IAuthorizationService authorizationService;
        private readonly IUnitOfWork unitOfWork;

        public UpdateShoppingListEntryCommandHandler(
            IRepository<UserGroup> userGroupRepository,
            IIdentityHelper identityHelper,
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork)
        {
            this.userGroupRepository = userGroupRepository;
            this.identityHelper = identityHelper;
            this.authorizationService = authorizationService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ShoppingListEntry> Handle(UpdateShoppingListEntryCommand command, CancellationToken cancellationToken)
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

            var shoppingListEntryToUpdate = userGroup.ShoppingListEntries
                .SingleOrDefault(entry => entry.Id == command.ShoppingListEntryId);
            if (shoppingListEntryToUpdate == null)
            {
                throw new EntityNotFoundException("ShoppingListEntry not found");
            }

            shoppingListEntryToUpdate.Name = command.Name;
            shoppingListEntryToUpdate.Quantity = command.Quantity;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return shoppingListEntryToUpdate;
        }
    }
}