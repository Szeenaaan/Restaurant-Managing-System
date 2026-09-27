using RMS.Application.Helpers;
using RMS.Repositories.AuthRepository;
using RMS.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RMS.Host.DI_Extentions
{
    public static class InjectionFiles
    {
        public static IServiceCollection InjectionFile(this IServiceCollection services)
        {
            // Helpers
            services.AddSingleton<JwtHelper>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
           {
               var jwtHelper = new JwtHelper();
               options.TokenValidationParameters = jwtHelper.GetTokenValidationParameters();
           });

            services.AddAuthorization();

            // Repositories
            services.AddScoped<AuthRepository>();

            // Services
            services.AddScoped<AuthService>();

            // Orchestrations

            return services;
        }
    }
}
