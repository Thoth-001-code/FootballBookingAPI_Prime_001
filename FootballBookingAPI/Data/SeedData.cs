
    using FootballBookingAPI.Models;
    using Microsoft.AspNetCore.Identity;
    using static FootballBookingAPI.Models.Enums.Enums;

namespace FootballBookingAPI.Data
{

    public static class SeedData
    {
        public static async Task SeedUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // ===== ROLES =====
            string[] roles = { "Admin", "Owner", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ===== ADMIN =====
            var adminEmail = "admin@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin",
                    Status = UserStatus.Active
                };

                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // ===== OWNER =====
            var ownerEmail = "owner@gmail.com";
            if (await userManager.FindByEmailAsync(ownerEmail) == null)
            {
                var owner = new ApplicationUser
                {
                    UserName = ownerEmail,
                    Email = ownerEmail,
                    FullName = "Owner",
                    Status = UserStatus.Active
                };

                await userManager.CreateAsync(owner, "Owner@123");
                await userManager.AddToRoleAsync(owner, "Owner");
            }

            // ===== USER =====
            var userEmail = "user@gmail.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FullName = "User",
                    Status = UserStatus.Active
                };

                await userManager.CreateAsync(user, "User@123");
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
