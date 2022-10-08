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
    public class DeleteShoppingListEntryCommandHandler
        : IRequestHandler<DeleteShoppingListEntryCommand>
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IIdentityHelper identityHelper;
        private readonly IUnitOfWork unitOfWork;

        public DeleteShoppingListEntryCommandHandler(
            IRepository<User> userRepository,
            IRepository<UserGroup> userGroupRepository,
            IAuthorizationService authorizationService,
            IIdentityHelper identityHelper,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.userGroupRepository = userGroupRepository;
            this.authorizationService = authorizationService;
            this.identityHelper = identityHelper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteShoppingListEntryCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(
                identityHelper.GetAuthenticatedUserId(),
                cancellationToken,
                nameof(User.Groups));
            var userGroup = user.Groups.SingleOrDefault(group => group.Id == command.GroupId);
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

            await userGroupRepository.LoadRelatedEntitiesAsync(
                userGroup,
                cancellationToken,
                nameof(UserGroup.ShoppingListEntries));
            var entryToDelete = userGroup.ShoppingListEntries
                .SingleOrDefault(group => group.Id == command.ShoppingListEntryId);
            if (entryToDelete == null)
            {
                return Unit.Value;
            }

            userGroup.RemoveShoppingListEntry(entryToDelete);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}