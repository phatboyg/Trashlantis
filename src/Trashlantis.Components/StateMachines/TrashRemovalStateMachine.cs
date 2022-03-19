namespace Trashlantis.Components.StateMachines
{
    using System;
    using Contracts;
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
                        x.Saga.BinNumber = x.Message.BinNumber;
                        x.Saga.RequestTimestamp = x.GetPayload<ConsumeContext>().SentTime ?? DateTime.UtcNow;
                    })
                    .PublishAsync(x => x.Init<EmptyTrashBin>(new { x.Message.BinNumber }))
                    .TransitionTo(Requested));

            During(Requested,
                When(TrashRemovalRequested)
                    .PublishAsync(x => x.Init<EmptyTrashBin>(new { x.Message.BinNumber })));
        }

        //
        // ReSharper disable UnassignedGetOnlyAutoProperty
        // ReSharper disable MemberCanBePrivate.Global
        public State Requested { get; }

        public Event<TakeOutTheTrash> TrashRemovalRequested { get; }
    }
}