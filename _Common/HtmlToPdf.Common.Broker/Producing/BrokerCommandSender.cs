using HtmlToPdf.Common.Broker.Producing.Interfaces;
using MassTransit;

namespace HtmlToPdf.Common.Broker.Producing;

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