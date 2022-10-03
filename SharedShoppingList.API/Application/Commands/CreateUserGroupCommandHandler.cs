using MediatR;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;

namespace SharedShoppingList.API.Application.Commands
{
    public class CreateUserGroupCommandHandler : IRequestHandler<CreateUserGroupCommand, UserGroup>
    {
        private readonly IRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserGroupCommandHandler(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserGroup> Handle(CreateUserGroupCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken,
                nameof(User.Groups));
            var userGroup = new UserGroup
            {
                Name = command.Name,
            };
            user.AddUserGroup(userGroup);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return userGroup;
        }
    }
}