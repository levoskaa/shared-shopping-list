using MediatR;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands.ShoppingListEntryCommands
{
    public class CreateShoppingListEntryCommand : IRequest<ShoppingListEntry>
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public int GroupId { get; set; }
    }
}