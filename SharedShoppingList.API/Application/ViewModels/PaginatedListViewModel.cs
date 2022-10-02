namespace SharedShoppingList.API.Application.ViewModels
{
    public class PaginatedListViewModel<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}