using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Core.Models;

namespace UserApi.Data
{
    public static class IdentitySeeder
    {
        public const string RoleAdmin = "Admin";
        public const string RoleOperator = "Operator";

        /// <summary>
        /// Creates default roles and optionally promotes a configured email to Admin/Operator.
        /// appsettings.json:
        /// "AdminSeed": { "AdminEmail": "...", "OperatorEmail": "..." }
        /// </summary>
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var config = sp.GetRequiredService<IConfiguration>();

            foreach (var role in new[] { RoleAdmin, RoleOperator })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = config["AdminSeed:AdminEmail"];
            if (!string.IsNullOrWhiteSpace(adminEmail))
            {
                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin != null && !await userManager.IsInRoleAsync(admin, RoleAdmin))
                    await userManager.AddToRoleAsync(admin, RoleAdmin);
            }

            var operatorEmail = config["AdminSeed:OperatorEmail"];
            if (!string.IsNullOrWhiteSpace(operatorEmail))
            {
                var op = await userManager.FindByEmailAsync(operatorEmail);
                if (op != null && !await userManager.IsInRoleAsync(op, RoleOperator))
                    await userManager.AddToRoleAsync(op, RoleOperator);
            }

        }

    }
}
