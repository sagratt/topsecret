using HtmlToPdf.Common.Broker.Consuming;
using HtmlToPdf.Common.Broker.Contracts.Commands;
using MassTransit;

namespace HtmlToPdf.ConversionService.Broker.Consuming.Consumers;

public class ConvertFileToPdfCommandConsumer : BaseConsumer<ConvertFileToPdfCommandConsumer, ConvertFileToPdfCommand>
{
    public ConvertFileToPdfCommandConsumer(ILogger<ConvertFileToPdfCommandConsumer> logger)
        : base(logger)
    {
    }

    protected override async Task ProcessMessage(ConsumeContext<ConvertFileToPdfCommand> context)
    {
        throw new NotImplementedException();
    }
}