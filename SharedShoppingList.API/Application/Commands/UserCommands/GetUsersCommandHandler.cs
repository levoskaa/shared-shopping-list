using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;

namespace SharedShoppingList.API.Application.Commands.UserCommands
{
    public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, PaginatedList<User>>
    {
        private readonly SharedShoppingListContext dbContext;

        public GetUsersCommandHandler(SharedShoppingListContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<PaginatedList<User>> Handle(GetUsersCommand command, CancellationToken cancellationToken)
        {
            IQueryable<User> query = dbContext.Users
                .Include(user => user.Groups);
            return PaginatedList<User>.CreateAsync(
                query,
                command.PageSize,
                command.Offset);
        }
    }
}