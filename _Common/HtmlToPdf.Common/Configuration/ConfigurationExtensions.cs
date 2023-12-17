using HtmlToPdf.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace HtmlToPdf.Common.Configuration;

public static class ConfigurationExtensions
{
    public static T GetSection<T>(this IConfiguration configuration)
        where T : class
    {
        var type = typeof(T);
        var configurationSection = configuration.GetSection(type.Name).Get(type) as T;

        if (configurationSection == null)
        {
            throw new ConfigurationVerificationException(type.Name);
        }

        return configurationSection;
    }
}