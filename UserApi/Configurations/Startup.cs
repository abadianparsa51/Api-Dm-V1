using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserApi.Configuration;
using UserApi.Core.Models;
using UserApi.Data;
using UserApi.Helper;

namespace UserApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Core Services
            services.AddCoreServices(Configuration);

            // Add Swagger Configuration
            services.AddSwaggerConfiguration();

            // Add JWT Authentication
            services.AddJwtAuthentication(Configuration);

            // Configure Hangfire
            services.AddHangfireConfiguration(Configuration);
            services.AddHttpContextAccessor();
            // Add CORS Configuration
            services.AddCorsConfiguration();

            // Add Controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1"));
            }

            // Enable Hangfire Dashboard
            app.UseHangfireDashboard("/hangfire");

            IdentitySeeder.SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();
            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //    var user = userManager.FindByEmailAsync("ParsaT73@routine.com").GetAwaiter().GetResult();

            //    if (user != null)
            //    {
            //        var token = userManager.GeneratePasswordResetTokenAsync(user).GetAwaiter().GetResult();
            //        var result = userManager.ResetPasswordAsync(user, token, "Admin@123456").GetAwaiter().GetResult();

            //        // اگر قبلاً پسورد داشته و Reset fail شد، لاگ بگیر (اختیاری)
            //        // مثلا Debug.WriteLine(string.Join(",", result.Errors.Select(e => e.Description)));
            //    }
            //}

            // Middleware Pipeline
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapControllers();
            });
        }

    }
}
