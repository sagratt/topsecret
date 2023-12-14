namespace HtmlToPdf.ConversionService.Business.Services.Interfaces;

public interface IFileConversionService
{
    Task ConvertToPdf(Guid fileId, string fileName);
}