using HtmlToPdf.ConversionService.Business.Configuration;
using HtmlToPdf.ConversionService.Business.Services;
using HtmlToPdf.ConversionService.Business.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HtmlToPdf.Common.Configuration;

namespace HtmlToPdf.ConversionService.Business.DI;

public static class BusinessServicesRegistrationExtensions
{
    public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        var puppeteerConfiguration = configuration.GetSection<PuppeteerConfiguration>();

        services.AddSingleton(puppeteerConfiguration);
        
        services.AddScoped<IFileConversionService, PuppeteerConversionService>();
    }
}