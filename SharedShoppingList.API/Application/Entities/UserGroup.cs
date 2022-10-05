namespace SharedShoppingList.API.Application.Entities
{
    public class UserGroup : Entity
    {
        public string Name { get; set; }

        private readonly List<UserUserGroup> userUserGroups = new();
        public virtual IReadOnlyCollection<UserUserGroup> UserUserGroups => userUserGroups;

        private readonly List<User> members = new();
        public virtual IReadOnlyCollection<User> Members => members;

        private readonly List<ShoppingListEntry> shoppingListEntries = new();
        public virtual IReadOnlyCollection<ShoppingListEntry> ShoppingListEntries => shoppingListEntries;

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public void AddShoppingListEntry(ShoppingListEntry shoppingListEntry)
        {
            shoppingListEntries.Add(shoppingListEntry);
        }
    }
}