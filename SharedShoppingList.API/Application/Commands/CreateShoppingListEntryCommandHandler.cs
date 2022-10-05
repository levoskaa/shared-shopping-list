using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Application.Commands
{
    public class CreateShoppingListEntryCommandHandler
        : IRequestHandler<CreateShoppingListEntryCommand, ShoppingListEntry>
    {
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IIdentityHelper identityHelper;
        private readonly IAuthorizationService authorizationService;
        private readonly IUnitOfWork unitOfWork;

        public CreateShoppingListEntryCommandHandler(
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

        public async Task<ShoppingListEntry> Handle(CreateShoppingListEntryCommand command, CancellationToken cancellationToken)
        {
            var userGroup = await userGroupRepository.GetByIdAsync(
                command.GroupId,
                cancellationToken);
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

            var shoppingListEntry = new ShoppingListEntry
            {
                Name = command.Name,
                Quantity = command.Quantity
            };
            userGroup.AddShoppingListEntry(shoppingListEntry);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return shoppingListEntry;
        }
    }
}