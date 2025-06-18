using Microsoft.Extensions.DependencyInjection;
using Services.Interface;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services)
        {
            // Register your services here
            // Example services - adjust according to your actual service interfaces and implementations
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokensService, TokenService>();
            // Add any other services your application needs
            // services.AddScoped<IEmailService, EmailService>();
            // services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}
