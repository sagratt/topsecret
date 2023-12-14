using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace HtmlToPdf.ConversionApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FileController : ControllerBase
{
    private readonly IConvertFileToPdfCommandSender _convertFileToPdfCommandSender;

    public FileController(IConvertFileToPdfCommandSender convertFileToPdfCommandSender)
    {
        _convertFileToPdfCommandSender = convertFileToPdfCommandSender;
    }

    [HttpPost]
    public async Task<IActionResult> Upload()
    {
        var request = HttpContext.Request;

        // validation of Content-Type
        // 1. first, it must be a form-data request
        // 2. a boundary should be found in the Content-Type
        if (!request.HasFormContentType ||
            !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
            string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
        {
            return new UnsupportedMediaTypeResult();
        }

        var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary.Value).Value;
        var reader = new MultipartReader(boundary, request.Body);
        var section = await reader.ReadNextSectionAsync();

        // This sample try to get the first file from request and save it
        // Make changes according to your needs in actual use
        while (section != null)
        {
            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                out var contentDisposition);

            if (hasContentDispositionHeader && contentDisposition!.DispositionType.Equals("form-data") &&
                !string.IsNullOrEmpty(contentDisposition.FileName.Value))
            {
                // Don't trust any file name, file extension, and file data from the request unless you trust them completely
                // Otherwise, it is very likely to cause problems such as virus uploading, disk filling, etc
                // In short, it is necessary to restrict and verify the upload
                // Here, we just use the temporary folder and a random file name

                // Get the temporary folder, and combine a random file name with it
                var fileId = Guid.NewGuid();
                var fileName = $"{Path.GetRandomFileName()}-{fileId}";
                var saveToPath = Path.Combine(Path.GetTempPath(), fileName);

                await using (var targetStream = System.IO.File.Create(saveToPath))
                {
                    await section.Body.CopyToAsync(targetStream);
                }
                
                Console.WriteLine($"Saved file to {saveToPath}.");

                await _convertFileToPdfCommandSender.Send(new ConvertFileToPdfCommand(fileId, fileName));
                
                Console.WriteLine($"Sent {nameof(ConvertFileToPdfCommand)}.");

                return Ok();
            }

            section = await reader.ReadNextSectionAsync();
        }

        // If the code runs to this location, it means that no files have been saved
        return BadRequest("No files data in the request.");
    }

    [HttpGet]
    public async Task<IActionResult> CheckStatus(Guid fileId)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> Download(Guid fileId)
    {
        throw new NotImplementedException();
    }
}