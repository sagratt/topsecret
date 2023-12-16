using HtmlToPdf.Common.Broker.Consuming.BaseConsumer;
using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.Common.Domain.Enums;
using HtmlToPdf.Common.ErrorMessages;
using HtmlToPdf.Common.Exceptions;
using HtmlToPdf.ConversionApi.Data.AppDatabase.Context;
using MassTransit;
using File = HtmlToPdf.ConversionApi.Data.AppDatabase.Entities.File;

namespace HtmlToPdf.ConversionApi.Broker.Consuming.Consumers;

using File = File;

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