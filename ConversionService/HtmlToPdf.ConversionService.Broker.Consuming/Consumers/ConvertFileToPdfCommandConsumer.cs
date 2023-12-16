using HtmlToPdf.Common.Broker.Consuming.BaseConsumer;
using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.ConversionService.Broker.Producing.EventPublishers.Interfaces;
using HtmlToPdf.ConversionService.Business.Services.Interfaces;
using MassTransit;

namespace HtmlToPdf.ConversionService.Broker.Consuming.Consumers;

public class ConvertFileToPdfCommandConsumer : BaseConsumer<ConvertFileToPdfCommandConsumer, ConvertFileToPdfCommand>
{
    private readonly IFileConversionService _fileConversionService;
    private readonly IConversionCompletedEventPublisher _conversionCompletedEventPublisher;
    private readonly IConversionStartedEventPublisher _conversionStartedEventPublisher;
    
    public ConvertFileToPdfCommandConsumer(
        ILogger<ConvertFileToPdfCommandConsumer> logger,
        IFileConversionService fileConversionService,
        IConversionCompletedEventPublisher conversionCompletedEventPublisher,
        IConversionStartedEventPublisher conversionStartedEventPublisher)
        : base(logger)
    {
        _fileConversionService = fileConversionService;
        _conversionCompletedEventPublisher = conversionCompletedEventPublisher;
        _conversionStartedEventPublisher = conversionStartedEventPublisher;
    }

    protected override async Task ProcessMessage(ConsumeContext<ConvertFileToPdfCommand> context)
    {
        var command = context.Message;

        await _conversionStartedEventPublisher.Publish(new ConversionStartedEvent(command.FileId));

        var convertedFilePath = await _fileConversionService.ConvertToPdf(command.FilePath);
        
        await _conversionCompletedEventPublisher
            .Publish(new ConversionCompletedEvent(command.FileId, Success: true, FilePath:convertedFilePath));
    }

    protected override async Task OnFailure(ConsumeContext<ConvertFileToPdfCommand> context)
    {
        var command = context.Message;
        
        await _conversionCompletedEventPublisher.Publish(new ConversionCompletedEvent(command.FileId, Success: false));
    }
}