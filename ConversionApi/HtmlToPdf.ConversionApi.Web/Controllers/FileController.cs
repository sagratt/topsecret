using System.Net;
using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.Common.ErrorMessages;
using HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;
using HtmlToPdfService.Common.Domain.Enums;
using HtmlToPdfService.ConversionApi.Data.AppDatabase.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using File = HtmlToPdfService.ConversionApi.Data.AppDatabase.Entities.File;

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
        
        if (!request.HasFormContentType ||
            !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
            string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
        {
            return new UnsupportedMediaTypeResult();
        }

        var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary.Value).Value;
        var reader = new MultipartReader(boundary, request.Body);
        var section = await reader.ReadNextSectionAsync();
        
        ContentDispositionHeaderValue.TryParse(section?.ContentDisposition,
            out var firstContentDisposition);
        var originalFileName = firstContentDisposition?.FileName.Value;
        
        while (section != null)
        {
            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                out var contentDisposition);
            
            if (hasContentDispositionHeader && contentDisposition!.DispositionType.Equals("form-data") &&
                !string.IsNullOrEmpty(contentDisposition.FileName.Value))
            {
                var fileId = Guid.NewGuid();
                var fileName = fileId.ToString();
                var saveToPath = Path.Combine(Path.GetTempPath(), fileName);

                await using (var targetStream = System.IO.File.Create(saveToPath))
                {
                    await section.Body.CopyToAsync(targetStream);
                }

                _applicationDatabase.Add(new File
                {
                    Id = fileId,
                    OriginalFileName = originalFileName ?? "",
                    StoredFileName = fileName,
                    StoredFileLocation = saveToPath,
                    ConversionStatus = FileConversionStatus.ReadyForConversion
                });

                await _applicationDatabase.SaveChangesAsync();

                await _convertFileToPdfCommandSender.Send(new ConvertFileToPdfCommand(fileId, saveToPath));
                
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
            FileConversionStatus.Success => File(System.IO.File.OpenRead(file.ConvertedFileLocation!), "application/pdf", file.ConvertedFileName),
            FileConversionStatus.InProgress => StatusCode((int)HttpStatusCode.Processing),
            _ => BadRequest("Something went wrong")
        };
    }
}