using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkSafe.Api.Application.Services;
using WorkSafe.Api.Infrastructure.Data;
using WorkSafe.Api.Infrastructure.Repositories;

namespace WorkSafe.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)); // <- SQL Server

            services.AddScoped<IWorkstationRepository, WorkstationRepository>();
            services.AddScoped<WorkstationAppService>();

            return services;
        }
    }
}