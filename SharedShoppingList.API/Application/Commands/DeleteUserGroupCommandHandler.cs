using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Application.Commands
{
    public class DeleteUserGroupCommandHandler : IRequestHandler<DeleteUserGroupCommand>
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserGroupCommandHandler(
            IRepository<User> userRepository,
            IRepository<UserGroup> userGroupRepository,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.userGroupRepository = userGroupRepository;
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteUserGroupCommand command, CancellationToken cancellationToken)
        {
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
            var user = await userRepository.GetByIdAsync(
                command.UserId,
                default,
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
                claimsPrincipal,
                groupToDelete,
                new UserGroupOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            userGroupRepository.Delete(groupToDelete);
            await unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}