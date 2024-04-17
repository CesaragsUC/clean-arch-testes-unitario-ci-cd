
using CleanArch.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.InfraSctructure.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfraServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }

    }
}
