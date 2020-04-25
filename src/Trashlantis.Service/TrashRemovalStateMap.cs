namespace Trashlantis.Service
{
    using Components.StateMachines;
    using MassTransit.EntityFrameworkCoreIntegration.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;


    public class TrashRemovalStateMap :
        SagaClassMap<TrashRemovalState>
    {
        protected override void Configure(EntityTypeBuilder<TrashRemovalState> entity, ModelBuilder model)
        {
            entity.Property(x => x.BinNumber);
            entity.HasIndex(x => x.BinNumber).IsUnique();

            entity.Property(x => x.CurrentState);
        }
    }
}