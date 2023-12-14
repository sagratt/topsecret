using HtmlToPdf.ConversionService.Business.Services;
using HtmlToPdf.ConversionService.Business.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlToPdf.ConversionService.Business.DI;

public static class BusinessServicesRegistrationExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IFileConversionService, FileConversionService>();
    }
}