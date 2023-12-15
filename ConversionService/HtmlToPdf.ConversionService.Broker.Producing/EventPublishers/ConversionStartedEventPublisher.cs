using HtmlToPdf.Common.Broker.Contracts.Events;
using HtmlToPdf.Common.Broker.Producing.BaseEventPublisher;
using HtmlToPdf.ConversionService.Broker.Producing.EventPublishers.Interfaces;
using MassTransit;

namespace HtmlToPdf.ConversionService.Broker.Producing.EventPublishers;

public class ConversionStartedEventPublisher: BrokerEventPublisher<ConversionStartedEvent>, IConversionStartedEventPublisher
{
    public ConversionStartedEventPublisher(IPublishEndpoint publishEndpoint) : base(publishEndpoint)
    {
    }
}