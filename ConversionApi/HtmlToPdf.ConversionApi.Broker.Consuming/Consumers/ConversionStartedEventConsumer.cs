using HtmlToPdf.Common.Broker.Consuming.BaseConsumer;
using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.Common.ErrorMessages;
using HtmlToPdf.Common.Exceptions;
using HtmlToPdfService.Common.Domain.Enums;
using HtmlToPdfService.ConversionApi.Data.AppDatabase.Context;
using MassTransit;

namespace HtmlToPdf.ConversionApi.Broker.Consuming.Consumers;

using File = HtmlToPdfService.ConversionApi.Data.AppDatabase.Entities.File;

public class ConversionStartedEventConsumer : BaseConsumer<ConversionStartedEventConsumer, ConversionStartedEvent>
{
    private readonly ApplicationDatabaseContext _applicationDatabase;
    
    public ConversionStartedEventConsumer(ILogger<ConversionStartedEventConsumer> logger, ApplicationDatabaseContext applicationDatabase) : base(logger)
    {
        _applicationDatabase = applicationDatabase;
    }

    protected override async Task ProcessMessage(ConsumeContext<ConversionStartedEvent> context)
    {
        var message = context.Message;
        
        var file = await _applicationDatabase.GetFileById(message.FileId);
        if (file is null)
        {
            throw new BusinessException(ErrorMessages.EntityNotFound<File>(message.FileId));
        }

        file.ConversionStatus = FileConversionStatus.InProgress;

        _applicationDatabase.Update(file);

        await _applicationDatabase.SaveChangesAsync();
    }
}