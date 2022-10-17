using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Application.Commands.UserGroupCommands
{
    public class UpdateUserGroupCommandHandler : IRequestHandler<UpdateUserGroupCommand, UserGroup>
    {
        private readonly IRepository<User> userRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IIdentityHelper identityHelper;
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserGroupCommandHandler(
            IRepository<User> userRepository,
            IAuthorizationService authorizationService,
            IIdentityHelper identityHelper,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.authorizationService = authorizationService;
            this.identityHelper = identityHelper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserGroup> Handle(UpdateUserGroupCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository
                .GetByIdAsync(command.UserId, cancellationToken, nameof(User.Groups));
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            var userGroup = user.Groups.SingleOrDefault(group => group.Id == command.GroupId);
            if (userGroup == null)
            {
                throw new EntityNotFoundException("UserGroup not found");
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(
                identityHelper.ClaimsPrincipal,
                userGroup,
                new UserGroupOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            userGroup.Name = command.Name;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return userGroup;
        }
    }
}