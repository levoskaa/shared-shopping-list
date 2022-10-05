namespace SharedShoppingList.API.Application.Entities
{
    public class ShoppingListEntry : Entity
    {
        public string Name { get; set; }
        public string Quantity { get; set; }

        public int GroupId { get; set; }
    }
}