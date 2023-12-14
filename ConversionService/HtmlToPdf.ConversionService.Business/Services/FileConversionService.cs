using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.ConversionService.Broker.Producing.EventPublishers.Interfaces;
using HtmlToPdf.ConversionService.Business.Services.Interfaces;

namespace HtmlToPdf.ConversionService.Business.Services;

public class FileConversionService : IFileConversionService
{
    private readonly IConversionCompletedEventPublisher _conversionCompletedEventPublisher;

    public FileConversionService(IConversionCompletedEventPublisher conversionCompletedEventPublisher)
    {
        _conversionCompletedEventPublisher = conversionCompletedEventPublisher;
    }

    public async Task ConvertToPdf(Guid fileId, string fileName)
    {
        await _conversionCompletedEventPublisher.Publish(new ConversionCompletedEvent(fileId, fileName));
        
        Console.WriteLine($"Published {nameof(ConversionCompletedEvent)}");
    }
}