using HtmlToPdf.Common.Broker.BaseCommandSender.Interfaces;
using HtmlToPdf.Common.Broker.Contracts.Commands;

namespace HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;

public interface IConvertFileToPdfCommandSender : IBrokerCommandSender<ConvertFileToPdfCommand>
{
    
}