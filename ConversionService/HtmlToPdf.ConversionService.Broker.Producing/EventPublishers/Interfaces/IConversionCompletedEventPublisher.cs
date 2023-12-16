using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.Common.Broker.Producing.Interfaces;

namespace HtmlToPdf.ConversionService.Broker.Producing.EventPublishers.Interfaces;

public interface IConversionCompletedEventPublisher : IBrokerEventPublisher<ConversionCompletedEvent>
{
    
}