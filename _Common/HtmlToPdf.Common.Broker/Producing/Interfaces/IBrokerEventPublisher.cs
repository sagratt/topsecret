namespace HtmlToPdf.Common.Broker.Producing.Interfaces;

public interface IBrokerEventPublisher<in TEvent> where TEvent : class
{
    Task Publish(TEvent @event);
}