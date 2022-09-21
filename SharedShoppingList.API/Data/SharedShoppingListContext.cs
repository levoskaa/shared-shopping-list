using Microsoft.EntityFrameworkCore;

namespace SharedShoppingList.API.Data
{
    public class SharedShoppingListContext : DbContext
    {
        public SharedShoppingListContext(DbContextOptions<SharedShoppingListContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyEntityConfigurations(modelBuilder);
        }

        private static void ApplyEntityConfigurations(ModelBuilder modelBuilder)
        {
            // TODO
        }
    }
}
