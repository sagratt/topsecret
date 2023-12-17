namespace HtmlToPdf.Common.Exceptions;

public class ConfigurationVerificationException : Exception
{
    public ConfigurationVerificationException(string sectionName) : base($"{sectionName}: section not found")
    {
    }
}