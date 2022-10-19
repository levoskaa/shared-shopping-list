using MediatR;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Application.Commands.InviteCodeCommands
{
    public class JoinUserGroupCommandHandler : IRequestHandler<JoinUserGroupCommand, Unit>
    {
        private readonly IIdentityHelper identityHelper;
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public JoinUserGroupCommandHandler(
            IIdentityHelper identityHelper,
            IRepository<UserGroup> userGroupRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            this.identityHelper = identityHelper;
            this.userGroupRepository = userGroupRepository;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(JoinUserGroupCommand command, CancellationToken cancellationToken)
        {
            var userGroups = await userGroupRepository.GetAsync(
                group => group.InviteCode == command.InviteCode,
                null,
                cancellationToken);
            UserGroup userGroup;
            try
            {
                userGroup = userGroups.Single();
            }
            catch (Exception)
            {
                throw new DomainException("InviteCode invalid");
            }

            var user = await userRepository.GetByIdAsync(
                identityHelper.GetAuthenticatedUserId(),
                cancellationToken,
                nameof(User.Groups));
            user.AddUserGroup(userGroup);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}