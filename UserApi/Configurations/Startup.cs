using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserApi.Configuration;

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

            // Middleware Pipeline
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
