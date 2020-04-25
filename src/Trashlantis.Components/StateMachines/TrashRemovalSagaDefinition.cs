namespace Trashlantis.Components.StateMachines
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using GreenPipes;
    using MassTransit;
    using MassTransit.Definition;


    public class TrashRemovalSagaDefinition :
        SagaDefinition<TrashRemovalState>
    {
        public TrashRemovalSagaDefinition()
        {
            Endpoint(x => x.ConcurrentMessageLimit = 1);
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<TrashRemovalState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000, 5000, 30000));
            endpointConfigurator.UseInMemoryOutbox();
            endpointConfigurator.UseFilter(new CatchMeIfYouCan());
        }
    }


    public class CatchMeIfYouCan :
        IFilter<ConsumeContext>
    {
        public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
        {
            await next.Send(context).ConfigureAwait(false);

            Console.WriteLine("Hello");
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}