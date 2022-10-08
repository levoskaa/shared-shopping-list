using MediatR;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class UpdateShoppingListEntryCommand : IRequest<ShoppingListEntry>
    {
        public int ShoppingListEntryId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}