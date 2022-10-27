using Microsoft.AspNetCore.Identity;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            // Admin
            if (!(await roleManager.RoleExistsAsync(UserRole.Admin.ToString())))
            {
                var adminRole = new Role
                {
                    Id = "05d432ff-d20b-485d-b261-2b757b631169",
                    Name = UserRole.Admin.ToString().ToLower(),
                };
                await roleManager.CreateAsync(adminRole);
            }
            // User
            if (!(await roleManager.RoleExistsAsync(UserRole.User.ToString())))
            {
                var userRole = new Role
                {
                    Id = "9239f573-1ae4-4fa9-a780-31fa52f97bb3",
                    Name = UserRole.User.ToString().ToLower(),
                };
                await roleManager.CreateAsync(userRole);
            }
        }

        public static async Task SeedUsers(UserManager<User> userManager)
        {
            // Admin user
            var user = await userManager.FindByNameAsync("admin");
            if (user == null)
            {
                user = new User
                {
                    Id = "9daa06d4-3d96-437a-9619-3272f46ef914",
                    UserName = "admin",
                };
                await userManager.CreateAsync(user, "SecretPassword123");
                await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            }
        }
    }
}
