using MediatR;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands.UserCommands
{
    public class GetUsersCommand
        : PaginationCommand, IRequest<PaginatedList<User>>
    {
    }
}