namespace HtmlToPdf.Common.Broker.Contracts.Commands;

public record ConvertFileToPdfCommand(Guid FileId, string FilePath)
{
    public Guid CorrelationId => FileId;
}