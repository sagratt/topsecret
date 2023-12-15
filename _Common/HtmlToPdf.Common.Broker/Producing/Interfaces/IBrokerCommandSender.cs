namespace HtmlToPdf.Common.Broker.BaseCommandSender.Interfaces;

public interface IBrokerCommandSender<in TCommand> where TCommand : class
{
    Task Send(TCommand command);
}