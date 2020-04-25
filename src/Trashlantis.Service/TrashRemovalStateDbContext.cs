namespace Trashlantis.Service
{
    using System.Collections.Generic;
    using MassTransit.EntityFrameworkCoreIntegration;
    using MassTransit.EntityFrameworkCoreIntegration.Mappings;
    using Microsoft.EntityFrameworkCore;


    public class TrashRemovalStateDbContext :
        SagaDbContext
    {
        public TrashRemovalStateDbContext(DbContextOptions<TrashRemovalStateDbContext> options)
            : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new TrashRemovalStateMap(); }
        }
    }
}