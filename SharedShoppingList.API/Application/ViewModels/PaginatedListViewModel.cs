namespace SharedShoppingList.API.Application.ViewModels
{
    public class PaginatedListViewModel<T>
    {
        public int Offset { get; private set; }
        public int TotalItemCount { get; private set; }
        public IEnumerable<T> Items { get; set; }
    }
}