using HtmlToPdf.Common.Domain.Enums;

namespace HtmlToPdf.ConversionApi.Data.AppDatabase.Entities;

public class File
{
    public Guid Id { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string StoredFileName { get; set; } = null!;

    public string? ConvertedFileName { get; set; }

    public string StoredFileLocation { get; set; } = null!;
    
    public string? ConvertedFileLocation { get; set; }

    public FileConversionStatus ConversionStatus { get; set; }
}