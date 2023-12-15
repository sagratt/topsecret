using HtmlToPdf.Common.Broker.DI;
using HtmlToPdf.ConversionApi.Broker.Consuming.Consumers;
using HtmlToPdfService.ConversionApi.Data.AppDatabase.DI;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddCommonBrokerServices(config =>
        {
            config.AddConsumer<ConversionCompletedEventConsumer>();
            config.AddConsumer<ConversionStartedEventConsumer>();
        });

        services.AddAppDatabaseServices(hostContext.Configuration);
    })
    .Build();

await host.RunAsync();