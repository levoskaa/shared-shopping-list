using MediatR;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;

namespace SharedShoppingList.API.Application.Commands.UserGroupCommands
{
    public class CreateUserGroupCommandHandler : IRequestHandler<CreateUserGroupCommand, UserGroup>
    {
        private readonly IRepository<User> userRepository;
        private readonly IInviteCodeService inviteCodeService;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserGroupCommandHandler(
            IRepository<User> userRepository,
            IInviteCodeService inviteCodeService,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.inviteCodeService = inviteCodeService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserGroup> Handle(CreateUserGroupCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken,
                nameof(User.Groups));
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var userGroup = new UserGroup
            {
                Name = command.Name,
                OwnerId = command.UserId,
                InviteCode = inviteCodeService.GenerateInviteCode(),
        };
            user.AddUserGroup(userGroup);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return userGroup;
        }
    }
}