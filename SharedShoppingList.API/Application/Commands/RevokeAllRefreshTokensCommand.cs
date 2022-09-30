using MediatR;

namespace SharedShoppingList.API.Application.Commands
{
    public class RevokeAllRefreshTokensCommand : IRequest
    {
        public string UserId { get; set; }
    }
}