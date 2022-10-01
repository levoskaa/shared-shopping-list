namespace SharedShoppingList.API.Application.Entities
{
    public class ShoppingListEntry : Entity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public int GroupId { get; set; }
    }
}