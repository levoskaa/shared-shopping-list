namespace SharedShoppingList.API.Application.Dtos
{
    public class PaginatedListDto<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}