namespace SharedShoppingList.API.Application.ViewModels
{
    public class UserGroupDetailsViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Members { get; set; }
        public bool IsOwnedByUser { get; set; }
    }
}
