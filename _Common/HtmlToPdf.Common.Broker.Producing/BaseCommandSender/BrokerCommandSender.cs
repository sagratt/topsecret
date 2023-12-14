using Common.Broker.Producing.BaseCommandSender.Interfaces;
using MassTransit;

namespace HtmlToPdf.Common.Broker.Producing.BaseCommandSender;

public abstract class BrokerCommandSender<TCommand> : IBrokerCommandSender<TCommand>
    where TCommand : class
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    protected BrokerCommandSender(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Send(TCommand command)
    {
        await _sendEndpointProvider.Send(command);
    }
}