using System.Reflection;
using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.Common.Broker.NameFormatters;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HtmlToPdf.Common.Broker.DI;

public static class CommonBrokerServicesRegistrationExtensions
{
    private static readonly IEntityNameFormatter EntityNameFormatter = new KebabCaseEntityNameFormatter();
    
    public static void AddCommonBrokerServices(this IServiceCollection services, Action<IBusRegistrationConfigurator>? individualConfig = null)
    {
        ConfigureEndpointConventions("exchange");
        
        services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
            
            individualConfig?.Invoke(options);
            
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
                
                cfg.MessageTopology.SetEntityNameFormatter(EntityNameFormatter);
            });
        });
    }
    
    private static void ConfigureEndpointConventions(string address)
    {
        var commandsAssembly = typeof(ConvertFileToPdfCommand).Assembly;
        var commandsNamespace = typeof(ConvertFileToPdfCommand).Namespace;
        const string commandsSuffix = "Command";
        
        var commandTypes = commandsAssembly.GetTypes()
            .Where(t => t.IsClass && t.Namespace == commandsNamespace && t.Name.EndsWith(commandsSuffix))
            .ToList();

        foreach (var commandType in commandTypes)
        {
            var entityNameFormatterFormatEntityName =
                typeof(KebabCaseEntityNameFormatter).GetMethod(nameof(KebabCaseEntityNameFormatter.FormatEntityName))!;
            var entityNameFormatterFormatEntityNameGeneric =
                entityNameFormatterFormatEntityName.MakeGenericMethod(commandType);
            var formattedEntityName = entityNameFormatterFormatEntityNameGeneric.Invoke(EntityNameFormatter, null);

            var endpointConventionMap =
                typeof(EndpointConvention).GetMethod(nameof(EndpointConvention.Map),
                    BindingFlags.Public | BindingFlags.Static, new[] { typeof(Uri) })!;
            var endpointConventionMapGeneric = endpointConventionMap.MakeGenericMethod(commandType);
            endpointConventionMapGeneric.Invoke(null,
                new object?[]
                    { new Uri($"{address}:{formattedEntityName}") });
        }
    }
}