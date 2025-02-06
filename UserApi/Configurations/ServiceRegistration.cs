using Microsoft.EntityFrameworkCore;
using UserApi.Core.Interfaces;
using UserApi.Data.Repositories;
using UserApi.Helpers;
using UserApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UserApi.Core.Models;
using UserApi.Data;
using UserApi.Core.Services;
using UserApi.Core.Repositories;
using UserApi.Helper;

namespace UserApi.Configuration
{
    public static class ServiceRegistration
    {
        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DevConnection")));

            // Identity Configuration
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApiDbContext>()
                .AddDefaultTokenProviders();

            // Dependency Injection for Repositories and Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            // Register services and repositories with DI container
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ICardService, CardService>();
            // Register services and repositories with DI container
            services.AddScoped<ICardDetailRepository, CardDetailRepository>();
            services.AddScoped<ICardDetailService, CardDetailService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<OtpRepository>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddSingleton<IJwtHelper, JwtHelper>();
            services.AddAutoMapper(typeof(ContactProfile));
            services.AddSignalR(); // اضافه کردن SignalR
            // Bind JwtConfig to appsettings.json section
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtConfig>>().Value);
        }
    }
}
