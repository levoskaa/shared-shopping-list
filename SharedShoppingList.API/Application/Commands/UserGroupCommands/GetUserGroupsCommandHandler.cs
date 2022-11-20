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
        private readonly IRepository<UserGroup> userGroupRepository;

        public GetUserGroupsCommandHandler(
            IRepository<User> userRepository,
            IRepository<UserGroup> userGroupRepository)
        {
            this.userRepository = userRepository;
            this.userGroupRepository = userGroupRepository;
        }

        public async Task<PaginatedList<UserGroup>> Handle(GetUserGroupsCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository
                .GetByIdAsync(command.UserId, cancellationToken, nameof(User.Groups));
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            foreach (var group in user.Groups)
            {
                await userGroupRepository.LoadRelatedEntitiesAsync(group,
                    cancellationToken, nameof(group.Members));
            }

            return new PaginatedList<UserGroup>(
                user.Groups,
                user.Groups.Count,
                command.Offset);
        }
    }
}