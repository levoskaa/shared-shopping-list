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
    public class DeleteUserGroupCommandHandler : IRequestHandler<DeleteUserGroupCommand>
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IIdentityHelper identityHelper;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserGroupCommandHandler(
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

        public async Task<Unit> Handle(DeleteUserGroupCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken,
                nameof(User.Groups));
            var groupToDelete = user.Groups.SingleOrDefault(group => group.Id == command.UserGroupId);

            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            if (groupToDelete == null)
            {
                return Unit.Value;
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(
                identityHelper.ClaimsPrincipal,
                groupToDelete,
                new UserGroupOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            userGroupRepository.Delete(groupToDelete);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}