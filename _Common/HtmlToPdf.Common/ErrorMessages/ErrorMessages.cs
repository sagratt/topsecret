namespace HtmlToPdf.Common.ErrorMessages;

public static class ErrorMessages
{
    public static string EntityNotFound<T>(Guid id)
    {
        return $"The {nameof(T)} with Id: {id} was not found in the database";
    }
}