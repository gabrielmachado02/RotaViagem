using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RotaViagem.Application.DTOs;
using RotaViagem.Application.Services;
using RotaViagem.Application.Validators;
using RotaViagem.Domain.Interfaces;
using RotaViagem.Infrastructure.Data;
using RotaViagem.Infrastructure.Repositories;
using System.Reflection;

namespace RotaViagem.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRotaViagemServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRotaService, Application.Services.RotaService>();
            services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(RotaInputDtoValidator)));
            services.AddScoped<IValidator<RotaInputDto>, RotaInputDtoValidator>();
            services.AddScoped<IValidator<ConsultaRotaDto>, ConsultaRotaDtoValidator>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IRotaRepository, RotaRepository>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<RotaInputDto>, RotaInputDtoValidator>();
            services.AddScoped<IValidator<ConsultaRotaDto>, ConsultaRotaDtoValidator>();



            return services;
        }

        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            await DatabaseSeed.SeedDatabaseAsync(serviceProvider);
        }
    }
}