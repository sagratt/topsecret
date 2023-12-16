namespace HtmlToPdfService.ConversionApi.Data.AppDatabase.Entities;

using Common.Domain.Enums;

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