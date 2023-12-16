namespace HtmlToPdf.ConversionService.Business.Services.Interfaces;

public interface IFileConversionService
{
    Task<string> ConvertToPdf(string filePath);
}