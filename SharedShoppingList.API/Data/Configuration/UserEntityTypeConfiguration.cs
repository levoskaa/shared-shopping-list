using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Data.Configuration
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Many-to-many relationship between Users and UserGroups
            builder
                .HasMany(user => user.Groups)
                .WithMany(group => group.Members)
                .UsingEntity<UserUserGroup>(
                    join => join
                        .HasOne(join => join.Group)
                        .WithMany(group => group.UserUserGroups)
                        .HasForeignKey(join => join.GroupId),
                    join => join
                        .HasOne(join => join.User)
                        .WithMany(user => user.UserUserGroups)
                        .HasForeignKey(join => join.UserId),
                    join =>
                    {
                        join.HasKey(join => new { join.UserId, join.GroupId });
                    });
        }
    }
}