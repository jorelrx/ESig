using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

using SalarioWeb.Application.Mapping;
using SalarioWeb.Application.Validators;
using SalarioWeb.Data;
using SalarioWeb.Repositories;
using SalarioWeb.Repositories.Interfaces;
using SalarioWeb.Services;
using SalarioWeb.Services.Interfaces;

namespace SalarioWeb.Application.Configurations
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
        {
            // Configuração do Entity Framework Core
            services.AddDbContext<SalarioContext>(options =>
                options.UseSqlServer(connectionString));

            // Configuração do FluentValidation
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreatePessoaDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdatePessoaDTOValidator>();

            // Configuração do AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Registro dos repositórios
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IExcelImportRepository, ExcelImportRepository>();

            // Registro dos serviços
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IExcelImportService, ExcelImportService>();

            return services;
        }
    }
}
