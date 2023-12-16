namespace HtmlToPdf.Common.Domain.Enums;

public enum FileConversionStatus
{
    ReadyForConversion,
    InProgress,
    Success,
    Failure = 10
}