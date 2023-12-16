using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.Common.ErrorMessages;
using HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Net;
using HtmlToPdf.Common.Domain.Enums;
using HtmlToPdf.ConversionApi.Data.AppDatabase.Context;
using File = HtmlToPdf.ConversionApi.Data.AppDatabase.Entities.File;

namespace HtmlToPdf.ConversionApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FileController : ControllerBase
{
    private readonly IConvertFileToPdfCommandSender _convertFileToPdfCommandSender;
    private readonly ApplicationDatabaseContext _applicationDatabase;

    public FileController(
        IConvertFileToPdfCommandSender convertFileToPdfCommandSender,
        ApplicationDatabaseContext applicationDatabase)
    {
        _convertFileToPdfCommandSender = convertFileToPdfCommandSender;
        _applicationDatabase = applicationDatabase;
    }

    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public async Task<IActionResult> UploadHtml()
    {
        var request = HttpContext.Request;

        if (!request.HasFormContentType || !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader)
            || string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
        {
            return new UnsupportedMediaTypeResult();
        }

        var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary.Value).Value;
        var reader = new MultipartReader(boundary, request.Body);
        var section = await reader.ReadNextSectionAsync();

        var originalFileName = GetFileName(section);

        while (section != null)
        {
            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                out var contentDisposition);

            if (ReachedFileEnd(hasContentDispositionHeader, contentDisposition))
            {
                var (fileId, fileName, saveToPath) = CreateFileInfo();

                await using (var targetStream = System.IO.File.Create(saveToPath))
                {
                    await section.Body.CopyToAsync(targetStream);
                }

                await SaveFileInfoToDb(originalFileName, fileId, fileName, saveToPath);

                await SendConvertFileToPdfCommand(fileId, saveToPath);

                return Ok(new
                {
                    FileId = fileId
                });
            }

            section = await reader.ReadNextSectionAsync();
        }

        return BadRequest("No files data in the request.");
    }

    [HttpGet]
    public async Task<IActionResult> CheckStatus(Guid fileId)
    {
        var file = await _applicationDatabase.GetFileById(fileId);
        if (file is null)
        {
            return BadRequest(ErrorMessages.EntityNotFound<File>(fileId));
        }

        return Ok(new
        {
            FileId = file.Id,
            FileConversionStatus = file.ConversionStatus,
            FileConversionStatusDescription = file.ConversionStatus.ToString()
        });
    }

    [HttpGet]
    public async Task<IActionResult> DownloadPdf(Guid fileId)
    {
        var file = await _applicationDatabase.GetFileById(fileId);
        if (file is null)
        {
            return BadRequest(ErrorMessages.EntityNotFound<File>(fileId));
        }

        return file.ConversionStatus switch
        {
            FileConversionStatus.Success => File(
                System.IO.File.OpenRead(file.ConvertedFileLocation!),
                "application/pdf",
                file.ConvertedFileName),
            FileConversionStatus.InProgress => StatusCode((int)HttpStatusCode.Processing),
            _ => BadRequest("Something went wrong")
        };
    }

    #region Private methods

    async Task SaveFileInfoToDb(string originalFileName, Guid fileId, string fileName, string saveToPath)
    {
        _applicationDatabase.Add(new File
        {
            Id = fileId,
            OriginalFileName = originalFileName,
            StoredFileName = fileName,
            StoredFileLocation = saveToPath,
            ConversionStatus = FileConversionStatus.ReadyForConversion
        });

        await _applicationDatabase.SaveChangesAsync();
    }

    private async Task SendConvertFileToPdfCommand(Guid fileId, string saveToPath)
    {
        await _convertFileToPdfCommandSender.Send(new ConvertFileToPdfCommand(fileId, saveToPath));
    }

    private static bool ReachedFileEnd(bool hasContentDispositionHeader, ContentDispositionHeaderValue? contentDisposition)
    {
        return hasContentDispositionHeader && contentDisposition!.DispositionType.Equals("form-data") &&
               !string.IsNullOrEmpty(contentDisposition.FileName.Value);
    }

    private static (Guid fileId, string fileName, string saveToPath) CreateFileInfo()
    {
        var fileId = Guid.NewGuid();
        var fileName = fileId.ToString();
        var saveToPath = Path.Combine(Path.GetTempPath(), fileName);

        return (fileId, fileName, saveToPath);
    }

    private static string GetFileName(MultipartSection? multipartSection)
    {
        ContentDispositionHeaderValue.TryParse(multipartSection?.ContentDisposition, out var firstContentDisposition);
        var fileName = firstContentDisposition?.FileName.Value ?? "";

        return fileName;
    }

    #endregion
}