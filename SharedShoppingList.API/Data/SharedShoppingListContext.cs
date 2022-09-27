using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data.Configuration;

namespace SharedShoppingList.API.Data
{
    public class SharedShoppingListContext : IdentityDbContext<User, Role, string>
    {
        public SharedShoppingListContext(DbContextOptions<SharedShoppingListContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ApplyEntityConfigurations(builder);
        }

        private static void ApplyEntityConfigurations(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RefreshTokenEntityTypeConfiguration());
        }
    }
}