namespace HtmlToPdfService.ConversionApi.Data.AppDatabase.Entities;

using Common.Domain.Enums;

public class File
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public FileConversionStatus ConversionStatus { get; set; }
}