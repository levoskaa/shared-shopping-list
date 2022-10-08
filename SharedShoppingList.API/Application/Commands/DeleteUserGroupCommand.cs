using MediatR;

namespace SharedShoppingList.API.Application.Commands
{
    public class DeleteUserGroupCommand : IRequest
    {
        public string UserId { get; set; }
        public int UserGroupId { get; set; }
    }
}