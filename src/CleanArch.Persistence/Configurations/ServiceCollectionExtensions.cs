using CleanArch.Domain.Interfaces;
using CleanArch.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Persistence.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //// Caso queira usar SqlServer
            //services.AddDbContext<FuncionarioDbContext>(options =>
            //              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<FuncionarioDbContext>(options =>
               options.UseSqlite(configuration.GetConnectionString("Sqlite")));


            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();

        }
    }
}
