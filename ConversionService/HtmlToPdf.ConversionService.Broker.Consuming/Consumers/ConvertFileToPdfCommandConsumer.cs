using HtmlToPdf.Common.Broker.Consuming;
using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.ConversionService.Business.Services.Interfaces;
using MassTransit;

namespace HtmlToPdf.ConversionService.Broker.Consuming.Consumers;

public class ConvertFileToPdfCommandConsumer : BaseConsumer<ConvertFileToPdfCommandConsumer, ConvertFileToPdfCommand>
{
    private readonly IFileConversionService _fileConversionService;
    
    public ConvertFileToPdfCommandConsumer(
        ILogger<ConvertFileToPdfCommandConsumer> logger,
        IFileConversionService fileConversionService)
        : base(logger)
    {
        _fileConversionService = fileConversionService;
    }

    protected override async Task ProcessMessage(ConsumeContext<ConvertFileToPdfCommand> context)
    {
        var command = context.Message;

        await _fileConversionService.ConvertToPdf(command.FileId, command.FileName);
    }
}