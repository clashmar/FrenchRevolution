using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrenchRevolution.Infrastructure.Data;

public static class SeedIdentity
{
    public static async Task SeedAsync(
        IServiceProvider services,
        // TODO: Change to options pattern
        IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Ensure roles exist
        string[] roles = [Roles.Admin, Roles.Member];
        foreach (var roleName in roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }

        // Create admin user if it does not exist
        var adminEmail = configuration["Admin:Email"];       
        var adminPassword = configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            return;
        }

        var admin = await userManager.FindByEmailAsync(adminEmail);
        
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DisplayName = "Site Administrator"
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin user: {errors}");
            }

            await userManager.AddToRolesAsync(admin, [Roles.Admin, Roles.Member]);
        }
    }
}