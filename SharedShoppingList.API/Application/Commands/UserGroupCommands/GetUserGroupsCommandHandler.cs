using MediatR;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Application.Commands.UserGroupCommands
{
    public class GetUserGroupsCommandHandler : IRequestHandler<GetUserGroupsCommand, PaginatedList<UserGroup>>
    {
        private readonly IRepository<User> userRepository;

        public GetUserGroupsCommandHandler(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<PaginatedList<UserGroup>> Handle(GetUserGroupsCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository
                .GetByIdAsync(command.UserId, cancellationToken, nameof(User.Groups));
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            return new PaginatedList<UserGroup>(
                user.Groups,
                user.Groups.Count,
                command.PageSize,
                command.PageIndex);
        }
    }
}