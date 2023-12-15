namespace HtmlToPdf.Common.Broker.Contracts.Events;

public record ConversionCompletedEvent(Guid FileId, bool Success, string? FilePath = null)
{
    public Guid CorrelationId => FileId;
}