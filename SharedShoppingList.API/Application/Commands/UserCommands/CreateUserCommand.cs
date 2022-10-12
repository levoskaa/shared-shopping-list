using MediatR;
using SharedShoppingList.API.Application.Common;

namespace SharedShoppingList.API.Application.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<AuthenticationResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
