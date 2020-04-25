namespace Trashlantis.Components.StateMachines
{
    using System;
    using Automatonymous;


    public class TrashRemovalState :
        SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }

        public string BinNumber { get; set; }

        public DateTime RequestTimestamp { get; set; }
    }
}