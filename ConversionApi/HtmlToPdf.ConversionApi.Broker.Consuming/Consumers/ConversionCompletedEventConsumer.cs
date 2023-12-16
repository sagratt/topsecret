using HtmlToPdf.Common.Broker.Consuming.BaseConsumer;
using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.Common.ErrorMessages;
using HtmlToPdf.Common.Exceptions;
using HtmlToPdfService.Common.Domain.Enums;
using HtmlToPdfService.ConversionApi.Data.AppDatabase.Context;
using MassTransit;
using File = HtmlToPdfService.ConversionApi.Data.AppDatabase.Entities.File;

namespace HtmlToPdf.ConversionApi.Broker.Consuming.Consumers;

public class ConversionCompletedEventConsumer : BaseConsumer<ConversionCompletedEventConsumer, ConversionCompletedEvent>
{
    private readonly ApplicationDatabaseContext _applicationDatabase;
    
    public ConversionCompletedEventConsumer(
        ILogger<ConversionCompletedEventConsumer> logger,
        ApplicationDatabaseContext applicationDatabase) : base(logger)
    {
        _applicationDatabase = applicationDatabase;
    }

    protected override async Task ProcessMessage(ConsumeContext<ConversionCompletedEvent> context)
    {
        var message = context.Message;

        var file = await _applicationDatabase.GetFileById(message.FileId);
        if (file is null)
        {
            throw new BusinessException(ErrorMessages.EntityNotFound<File>(message.FileId));
        }

        if (!message.Success)
        {
            file.ConversionStatus = FileConversionStatus.Failure;
        }

        file.ConversionStatus = FileConversionStatus.Success;
        file.ConvertedFileLocation = message.FilePath;
        file.ConvertedFileName = Path.GetFileName(message.FilePath);

        _applicationDatabase.Update(file);

        await _applicationDatabase.SaveChangesAsync();
    }
}