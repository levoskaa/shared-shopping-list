namespace SharedShoppingList.API.Application.Commands
{
    public abstract class PaginationCommand
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}