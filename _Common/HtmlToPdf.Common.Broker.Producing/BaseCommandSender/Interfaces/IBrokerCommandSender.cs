namespace Common.Broker.Producing.BaseCommandSender.Interfaces;

public interface IBrokerCommandSender<in TCommand> where TCommand : class
{
    Task Send(TCommand command);
}