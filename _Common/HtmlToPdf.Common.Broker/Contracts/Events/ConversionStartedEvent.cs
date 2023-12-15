namespace HtmlToPdf.Common.Broker.Contracts.Events;

public record ConversionStartedEvent(Guid FileId)
{
    public Guid CorrelationId => FileId;
}