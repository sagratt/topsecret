namespace HtmlToPdf.Common.Broker.Producing.BaseEventPublisher.Interfaces;

public interface IBrokerEventPublisher<in TEvent> where TEvent : class
{
    Task Publish(TEvent @event);
}