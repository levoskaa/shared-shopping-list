using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Data.Configuration
{
    public class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            // One-to-many relationship between UserGroups and ShoppingListEntries
            builder
                .HasMany(group => group.ShoppingListEntries)
                .WithOne()
                .HasForeignKey(entry => entry.GroupId);
        }
    }
}