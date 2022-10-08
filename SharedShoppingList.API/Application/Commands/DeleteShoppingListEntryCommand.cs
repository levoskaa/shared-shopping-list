using MediatR;

namespace SharedShoppingList.API.Application.Commands
{
    public class DeleteShoppingListEntryCommand : IRequest
    {
        public int ShoppingListEntryId { get; set; }
        public int GroupId { get; set; }
    }
}