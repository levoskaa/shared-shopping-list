using MediatR;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignOutCommand : IRequest
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
