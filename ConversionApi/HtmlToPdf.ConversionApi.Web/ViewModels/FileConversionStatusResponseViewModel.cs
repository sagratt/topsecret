using HtmlToPdf.Common.Domain.Enums;

namespace HtmlToPdf.ConversionApi.WebApi.ViewModels;

public class FileConversionStatusResponseViewModel
{
    public Guid FileId { get; set; }
    
    public FileConversionStatus ConversionStatus { get; set; }

    public string ConversionStatusDescription => ConversionStatus.ToString();
}