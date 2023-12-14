using MassTransit;

namespace HtmlToPdf.Common.Broker.NameFormatters;

public class KebabCaseEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        var formatter = KebabCaseEndpointNameFormatter.Instance;
        var name = typeof(T).Name;
        var formattedName = $"{formatter.SanitizeName(name)}-outgoing";
        
        return formattedName;
    }
}