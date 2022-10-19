using MediatR;

namespace SharedShoppingList.API.Application.Commands.InviteCodeCommands
{
    public class JoinUserGroupCommand : IRequest
    {
        public string InviteCode { get; set; }
    }
}