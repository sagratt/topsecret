namespace HtmlToPdf.Common.Broker.Producing.Interfaces;

public interface IBrokerCommandSender<in TCommand> where TCommand : class
{
    Task Send(TCommand command);
}