using MediatR;
using SharedShoppingList.API.Application.Common;

namespace SharedShoppingList.API.Application.Commands
{
    public class RefreshAuthCommand : IRequest<AuthenticationResult>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}