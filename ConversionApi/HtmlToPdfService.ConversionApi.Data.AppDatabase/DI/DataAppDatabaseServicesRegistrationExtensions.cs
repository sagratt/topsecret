using HtmlToPdfService.ConversionApi.Data.AppDatabase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlToPdfService.ConversionApi.Data.AppDatabase.DI;

public static class DataAppDatabaseServicesRegistrationExtensions
{
    public static void AddAppDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDatabaseContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDatabaseContext)));
        });
    }
}