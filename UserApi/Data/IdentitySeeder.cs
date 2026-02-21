using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserApi.Core.Models;

namespace UserApi.Data
{
    public static class IdentitySeeder
    {
        public const string RoleAdmin = "Admin";       // یا "Administrator" اگر خواستی
        public const string RoleOperator = "Operator";

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

            // 1) Roles
            await EnsureRoleAsync(roleManager, RoleAdmin, logger);
            await EnsureRoleAsync(roleManager, RoleOperator, logger);

            // 2) Admin user
            var adminEmail = config["AdminSeed:AdminEmail"];
            var adminPassword = config["AdminSeed:AdminPassword"];
            var resetAdminPassword = config.GetValue("AdminSeed:ResetAdminPassword", false);

            if (!string.IsNullOrWhiteSpace(adminEmail))
            {
                var admin = await EnsureUserAsync(
                    userManager,
                    email: adminEmail,
                    password: adminPassword,
                    resetPassword: resetAdminPassword,
                    logger: logger
                );

                if (admin != null)
                    await EnsureUserInRoleAsync(userManager, admin, RoleAdmin, logger);
            }

            // 3) Operator user (اختیاری)
            var operatorEmail = config["AdminSeed:OperatorEmail"];
            var operatorPassword = config["AdminSeed:OperatorPassword"];
            var resetOperatorPassword = config.GetValue("AdminSeed:ResetOperatorPassword", false);

            if (!string.IsNullOrWhiteSpace(operatorEmail))
            {
                var op = await EnsureUserAsync(
                    userManager,
                    email: operatorEmail,
                    password: operatorPassword,
                    resetPassword: resetOperatorPassword,
                    logger: logger
                );

                if (op != null)
                    await EnsureUserInRoleAsync(userManager, op, RoleOperator, logger);
            }
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName, ILogger logger)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                return;

            var res = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!res.Succeeded)
                throw new Exception($"Failed to create role '{roleName}': {string.Join(" | ", res.Errors.Select(e => e.Description))}");

            logger.LogInformation("Role created: {Role}", roleName);
        }

        private static async Task<ApplicationUser?> EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string email,
            string? password,
            bool resetPassword,
            ILogger logger)
        {
            var user = await userManager.FindByEmailAsync(email);

            // اگر وجود داشت: فقط در صورت نیاز Reset
            if (user != null)
            {
                if (resetPassword && !string.IsNullOrWhiteSpace(password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetRes = await userManager.ResetPasswordAsync(user, token, password);

                    if (!resetRes.Succeeded)
                        throw new Exception($"Failed to reset password for '{email}': {string.Join(" | ", resetRes.Errors.Select(e => e.Description))}");

                    logger.LogWarning("Password reset for existing user: {Email}", email);
                }

                return user;
            }

            // اگر وجود نداشت: ایجاد کن
            if (string.IsNullOrWhiteSpace(password))
            {
                // اگر پسورد ندادی، می‌تونی یا Exception بدی یا یک پسورد random بسازی
                throw new Exception($"AdminSeed password is missing for '{email}'. Set AdminSeed:AdminPassword.");
            }

            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createRes = await userManager.CreateAsync(user, password);
            if (!createRes.Succeeded)
                throw new Exception($"Failed to create user '{email}': {string.Join(" | ", createRes.Errors.Select(e => e.Description))}");

            logger.LogInformation("User created: {Email}", email);
            return user;
        }

        private static async Task EnsureUserInRoleAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string role, ILogger logger)
        {
            if (await userManager.IsInRoleAsync(user, role))
                return;

            var res = await userManager.AddToRoleAsync(user, role);
            if (!res.Succeeded)
                throw new Exception($"Failed to add user '{user.Email}' to role '{role}': {string.Join(" | ", res.Errors.Select(e => e.Description))}");

            logger.LogInformation("Role assigned: {Email} -> {Role}", user.Email, role);
        }
    }
}