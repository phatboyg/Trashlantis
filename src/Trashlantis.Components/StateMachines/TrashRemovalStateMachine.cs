namespace Trashlantis.Components.StateMachines
{
    using System;
    using Automatonymous;
    using Contracts;
    using GreenPipes;
    using MassTransit;


    public class TrashRemovalStateMachine :
        MassTransitStateMachine<TrashRemovalState>
    {
        public TrashRemovalStateMachine()
        {
            InstanceState(instance => instance.CurrentState, Requested);

            Event(() => TrashRemovalRequested, x =>
            {
                x.CorrelateBy(instance => instance.BinNumber, context => context.Message.BinNumber);
                x.SelectId(context => NewId.NextGuid());
            });

            Initially(
                When(TrashRemovalRequested)
                    .Then(x =>
                    {
                        x.Instance.BinNumber = x.Data.BinNumber;
                        x.Instance.RequestTimestamp = x.GetPayload<ConsumeContext>().SentTime ?? DateTime.UtcNow;
                    })
                    .PublishAsync(x => x.Init<EmptyTrashBin>(new {x.Data.BinNumber}))
                    .TransitionTo(Requested));

            During(Requested,
                When(TrashRemovalRequested)
                    .PublishAsync(x => x.Init<EmptyTrashBin>(new {x.Data.BinNumber})));
        }

        public State Requested { get; private set; }

        public Event<TakeOutTheTrash> TrashRemovalRequested { get; private set; }
    }
}