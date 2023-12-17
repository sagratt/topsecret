using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.Common.Broker.Producing;
using HtmlToPdf.ConversionService.Broker.Producing.EventPublishers.Interfaces;
using MassTransit;

namespace HtmlToPdf.ConversionService.Broker.Producing.EventPublishers;

public class ConversionCompletedEventPublisher: BrokerEventPublisher<ConversionCompletedEvent>, IConversionCompletedEventPublisher
{
    public ConversionCompletedEventPublisher(IPublishEndpoint publishEndpoint) : base(publishEndpoint)
    {
    }
}