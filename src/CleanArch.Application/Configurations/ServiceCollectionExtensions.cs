using CleanArch.Application.Abstractions;
using CleanArch.Application.AutomapperConfig;
using CleanArch.Application.Common;
using CleanArch.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IFuncionarioService, FuncionarioServices>();
            services.AddScoped(typeof(IResult<>), typeof(Result<>));

            services.AddAutoMapper(typeof(AutoMapperConfig));


            // Registra os validators do FluentValidation, para serem injetados automaticamente
            // Todas classes que herdam de AbstractValidator e registrar no container
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
