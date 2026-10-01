using Microsoft.AspNetCore.Authentication.JwtBearer;
using RMS.Application.Helpers.IFileStorage;
using RMS.Application.Helpers.JwtHelper;
using RMS.Infrastructure.FileStorage;
using RMS.Repositories.AuthRepository;
using RMS.Repositories.CategoryRepository;
using RMS.Services.AuthService;
using RMS.Services.CategoryService;

namespace RMS.Host.DI_Extentions
{
    public static class InjectionFiles
    {
        public static IServiceCollection InjectionFile(this IServiceCollection services)
        {
            // Helpers

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
           {
               options.TokenValidationParameters = JwtHelper.GetTokenValidationParameters();
           });

            services.AddAuthorization();

            // Repositories
            services.AddScoped<AuthRepository>();
            services.AddScoped<CategoryRepository>();

            // Services
            services.AddScoped<AuthService>();
            services.AddScoped<CategoryService>();

            // Orchestrations

            //Infrastructure
            services.AddScoped<IFileStorage, FileStorageHelper>();
            return services;

        }
    }
}
