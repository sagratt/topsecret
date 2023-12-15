using HtmlToPdf.Common.Broker.Producing.BaseEventPublisher.Interfaces;
using MassTransit;

namespace HtmlToPdf.Common.Broker.Producing.BaseEventPublisher;

public class BrokerEventPublisher<TEvent> : IBrokerEventPublisher<TEvent>
    where TEvent : class
{
    private readonly IPublishEndpoint _publishEndpoint;

    protected BrokerEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Publish(TEvent @event)
    {
        await _publishEndpoint.Publish(@event);
    }
}