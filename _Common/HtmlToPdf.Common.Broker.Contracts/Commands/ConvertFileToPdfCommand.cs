namespace HtmlToPdf.Common.Broker.Contracts.Commands;

public record ConvertFileToPdfCommand(Guid FileId, string FileName)
{
    public Guid CorrelationId => FileId;
}