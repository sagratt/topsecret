namespace HtmlToPdf.Common.ErrorMessages;

public static class ErrorMessages
{
    public static string EntityNotFound<T>(Guid id)
    {
        return $"The {nameof(T)} with Id: {id} was not found in the database";
    }

    public static string FileIsNotReadyForDownload(Guid fileId) => $"File with Id: {fileId} is not ready for download.";

    public const string NoFileInRequest = "No file found in this request.";
}