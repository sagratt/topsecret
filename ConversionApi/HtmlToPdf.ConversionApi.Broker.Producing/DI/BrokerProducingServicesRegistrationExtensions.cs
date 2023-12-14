using HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders;
using HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlToPdf.ConversionApi.Broker.Producing.DI;

public static class BrokerProducingServicesRegistrationExtensions
{
    public static void AddBrokerProducingServices(this IServiceCollection services)
    {
        services.AddScoped<IConvertFileToPdfCommandSender, ConvertFileToPdfCommandSender>();
    }
}