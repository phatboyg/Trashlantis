namespace Trashlantis.Components.StateMachines
{
    using System;
    using MassTransit;


    public class TrashRemovalState :
        SagaStateMachineInstance
    {
        public int CurrentState { get; set; }

        public string BinNumber { get; set; }

        public DateTime RequestTimestamp { get; set; }
        public Guid CorrelationId { get; set; }
    }
}