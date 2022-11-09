using Microsoft.EntityFrameworkCore;

namespace SharedShoppingList.API.Application.Common
{
    public class PaginatedList<T>
    {
        public int Offset { get; private set; }
        public int ItemCount { get; private set; }
        private readonly List<T> items = new List<T>();
        public IReadOnlyCollection<T> Items => items;

        public PaginatedList(IEnumerable<T> items, int count, int offset)
        {
            ItemCount = count;
            Offset = offset;
            this.items.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageSize, int offset)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<T>(items, count, offset);
        }
    }
}