using MediatR;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Application.Commands
{
    public class GetUserGroupCommandHandler : IRequestHandler<GetUserGroupCommand, UserGroup>
    {
        private readonly IRepository<User> userRepository;

        public GetUserGroupCommandHandler(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserGroup> Handle(GetUserGroupCommand command, CancellationToken cancellationToken)
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
            return userGroup;
        }
    }
}