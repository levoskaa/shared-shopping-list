using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Application.Commands
{
    public class GetUserGroupsCommandHandler : IRequestHandler<GetUserGroupsCommand, PaginatedList<UserGroup>>
    {
        private readonly UserManager<User> userManager;

        public GetUserGroupsCommandHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<PaginatedList<UserGroup>> Handle(GetUserGroupsCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.Users
                .Include(user => user.Groups)
                .SingleOrDefaultAsync(user => user.Id == command.UserId && user.UserName == command.Username);
            if (user == null)
            {
                throw new ForbiddenException();
            }
            return new PaginatedList<UserGroup>(
                user.Groups,
                user.Groups.Count,
                command.PageSize,
                command.PageIndex);
        }
    }
}