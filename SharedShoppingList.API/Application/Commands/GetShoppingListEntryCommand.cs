using MediatR;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class GetShoppingListEntryCommand : IRequest<ShoppingListEntry>
    {
        public int GroupId { get; set; }
        public int ShoppingListEntryId { get; set; }
    }
}