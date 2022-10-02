using Microsoft.EntityFrameworkCore;

namespace SharedShoppingList.API.Application.Common
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        private readonly List<T> items = new List<T>();
        public IReadOnlyCollection<T> Items => items;

        public PaginatedList(IEnumerable<T> items, int count, int pageSize, int pageIndex)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.items.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageSize, int pageIndex)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<T>(items, count, pageSize, pageIndex);
        }
    }
}