namespace Trashlantis.Components
{
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.Extensions.Logging;


    public class TrashConsumer :
        IConsumer<EmptyTrashBin>
    {
        readonly ILogger<TrashConsumer> _logger;

        public TrashConsumer(ILogger<TrashConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EmptyTrashBin> context)
        {
            _logger.LogInformation("Emptying Trash bin: {BinNumber}", context.Message.BinNumber);

            await Task.Delay(1000);
        }
    }
}