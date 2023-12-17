using MassTransit;
using Microsoft.Extensions.Logging;

namespace HtmlToPdf.Common.Broker.Consuming.BaseConsumer;

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

    protected virtual Task OnFailure(ConsumeContext<TMessage> context)
    {
        return Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        try
        {
            _logger.LogInformation(
                $"Message Broker Consume START [{typeof(TConsumer).Name}]. {Environment.NewLine}" +
                $"CorrelationId: {context.CorrelationId} {Environment.NewLine}" +
                $"Consumed message type: {typeof(TMessage).Name} {Environment.NewLine}");

            await ProcessMessage(context);
        }
        catch (Exception exception)
        {
            await OnFailure(context);

            _logger.LogError(exception,
                $"Message Broker Consume ERROR [{typeof(TConsumer).Name}]. {Environment.NewLine}" +
                $"CorrelationId: {context.CorrelationId} {Environment.NewLine}" +
                $"Consumed message type: {typeof(TMessage).Name} {Environment.NewLine}" +
                $"Error message: {exception.Message}");
        }
    }
}
