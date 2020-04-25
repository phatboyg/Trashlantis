namespace Trashlantis.Service
{
    using System.IO;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;


    public class TrashRemovalStateDbContextFactory :
        IDesignTimeDbContextFactory<TrashRemovalStateDbContext>
    {
        public TrashRemovalStateDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TrashRemovalStateDbContext>();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("service"), m =>
            {
                m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                m.MigrationsHistoryTable($"__{nameof(TrashRemovalStateDbContext)}");
            });


            return new TrashRemovalStateDbContext(optionsBuilder.Options);
        }
    }
}