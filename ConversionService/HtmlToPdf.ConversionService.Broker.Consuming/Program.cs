using HtmlToPdf.Common.Broker.DI;
using HtmlToPdf.ConversionService.Broker.Consuming.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCommonBrokerServices(config =>
        {
            config.AddConsumer<ConvertFileToPdfCommandConsumer>();
        });
    })
    .Build();

await host.RunAsync();