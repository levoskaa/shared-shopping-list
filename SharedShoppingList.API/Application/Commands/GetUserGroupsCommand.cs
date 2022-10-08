using MediatR;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class GetUserGroupsCommand :
        PaginationCommand, IRequest<PaginatedList<UserGroup>>
    {
        public string UserId { get; set; }
    }
}