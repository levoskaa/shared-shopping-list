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

            // Many-to-one relationship between UserGroups and Users
            builder
                .HasOne(group => group.Owner)
                .WithMany()
                .HasForeignKey(group => group.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}