using MediatR;

namespace SharedShoppingList.API.Application.Commands.InviteCodeCommands
{
    public class GetInviteCodeCommand : IRequest<string>
    {
        public int GroupId { get; set; }
    }
}