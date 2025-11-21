using Microsoft.AspNetCore.Identity;

namespace TiendaDavid.Identity.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider service, IConfiguration configuration, ILogger logger)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<IdentityUser>>();

            foreach (var role in Roles.GetAllRoles())
            {

                if (!await roleManager.RoleExistsAsync(role))
                {
                    logger.LogInformation("Role '{Role}' created successfully", role);
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = configuration["AdminUser:Email"]!;
            var adminPassword = configuration["AdminUser:Password"];
            var adminUserName = configuration["AdminUser:UserName"];

            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var user = new IdentityUser
                {
                    Email = adminEmail,
                    UserName = adminUserName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    logger.LogInformation("Admin user '{Email}' created succesfully with Admin role.", adminEmail);
                }
            }
        }
    }
}
