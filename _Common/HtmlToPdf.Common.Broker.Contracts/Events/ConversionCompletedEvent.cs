namespace HtmlToPdf.Common.Broker.Contracts.Events;

public record ConversionCompletedEvent(Guid FileId, string FileName)
{
    public Guid CorrelationId => FileId;
}