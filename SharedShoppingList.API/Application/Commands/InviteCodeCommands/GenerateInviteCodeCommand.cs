using MediatR;

namespace SharedShoppingList.API.Application.Commands.InviteCodeCommands
{
    public class GenerateInviteCodeCommand : IRequest<string>
    {
        public int GroupId { get; set; }
    }
}