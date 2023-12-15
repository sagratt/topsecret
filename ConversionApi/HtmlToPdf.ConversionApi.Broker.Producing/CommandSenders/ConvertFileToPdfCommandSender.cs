using HtmlToPdf.Common.Broker.Contracts.Commands;
using HtmlToPdf.Common.Broker.Producing.BaseCommandSender;
using HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders.Interfaces;
using MassTransit;

namespace HtmlToPdf.ConversionApi.Broker.Producing.CommandSenders;

public class ConvertFileToPdfCommandSender : BrokerCommandSender<ConvertFileToPdfCommand>, IConvertFileToPdfCommandSender
{
    public ConvertFileToPdfCommandSender(ISendEndpointProvider sendEndpointProvider)
        : base(sendEndpointProvider)
    {
    }
}