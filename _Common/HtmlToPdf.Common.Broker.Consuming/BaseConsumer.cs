using MassTransit;
using Microsoft.Extensions.Logging;

namespace HtmlToPdf.Common.Broker.Consuming;

public abstract class BaseConsumer<TConsumer, TMessage> : IConsumer<TMessage>
    where TMessage : class
    where TConsumer : IConsumer<TMessage>
{
    private readonly ILogger<TConsumer> _logger;

    protected BaseConsumer(ILogger<TConsumer> logger)
    {
        _logger = logger;
    }

    protected abstract Task ProcessMessage(ConsumeContext<TMessage> context);

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        try
        {
            await ProcessMessage(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                $"Message Broker Consume ERROR [{typeof(TConsumer).Name}]. {Environment.NewLine}" +
                $"CorrelationId: {context.CorrelationId} {Environment.NewLine}" +
                $"Consumed message type: {typeof(TMessage).Name} {Environment.NewLine}" +
                $"Error message: {exception.Message}");
        }
    }
}
