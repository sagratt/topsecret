using HtmlToPdf.ConversionService.Broker.Producing.EventPublishers;
using HtmlToPdf.ConversionService.Broker.Producing.EventPublishers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlToPdf.ConversionService.Broker.Producing.DI;

public static class BrokerProducingServicesRegistrationExtensions
{
    public static void AddBrokerProducingServices(this IServiceCollection services)
    {
        services.AddScoped<IConversionCompletedEventPublisher, ConversionCompletedEventPublisher>();
    }
}