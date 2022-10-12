using MediatR;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands.UserGroupCommands
{
    public class GetUserGroupCommand : IRequest<UserGroup>
    {
        public string UserId { get; set; }
        public int GroupId { get; set; }
    }
}