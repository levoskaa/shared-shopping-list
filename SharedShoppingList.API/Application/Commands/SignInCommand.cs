using MediatR;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignInCommand : IRequest<Token>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
