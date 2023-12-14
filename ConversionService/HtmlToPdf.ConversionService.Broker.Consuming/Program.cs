using HtmlToPdf.Common.Broker.DI;
using HtmlToPdf.ConversionService.Broker.Consuming.Consumers;
using HtmlToPdf.ConversionService.Broker.Producing.DI;
using HtmlToPdf.ConversionService.Business.DI;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCommonBrokerServices(config =>
        {
            config.AddConsumer<ConvertFileToPdfCommandConsumer>();
        });

        services.AddBrokerProducingServices();
        services.AddBusinessServices();
    })
    .Build();

await host.RunAsync();