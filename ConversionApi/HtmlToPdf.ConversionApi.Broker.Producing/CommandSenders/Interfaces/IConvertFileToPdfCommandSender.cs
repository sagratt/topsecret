using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.Common.Broker.Producing.Interfaces;

namespace HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;

public interface IConvertFileToPdfCommandSender : IBrokerCommandSender<ConvertFileToPdfCommand>
{
    
}