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
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IOtpRepository, OtpRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ICardDetailRepository, CardDetailRepository>();
            services.AddScoped<ICardDetailService, CardDetailService>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IContactCommandRepository, ContactCommandRepository>();
            services.AddScoped<OtpRepository>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddSingleton<IJwtHelper, JwtHelper>();

            // Register AutoMapper Profile
            services.AddAutoMapper(typeof(ContactProfile));

            // Register SignalR
            services.AddSignalR();

            // Bind JwtConfig to appsettings.json section
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtConfig>>().Value);

            // Register MediatR correctly
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly); // Registers handlers and other components from the same assembly
            });
        }
    }
}
