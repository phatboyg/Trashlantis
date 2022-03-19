namespace Trashlantis.Service
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Components;
    using Components.StateMachines;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Events;


    public static class Program
    {
        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();
                        x.AddConsumer<TrashConsumer>();

                        x.AddSagaStateMachine<TrashRemovalStateMachine, TrashRemovalState, TrashRemovalSagaDefinition>()
                            .EntityFrameworkRepository(r =>
                            {
                                r.ConcurrencyMode = ConcurrencyMode.Pessimistic;

                                r.AddDbContext<DbContext, TrashRemovalStateDbContext>((provider, optionsBuilder) =>
                                {
                                    optionsBuilder.UseSqlServer(hostContext.Configuration.GetConnectionString("service"), m =>
                                    {
                                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                        m.MigrationsHistoryTable($"__{nameof(TrashRemovalStateDbContext)}");
                                    });
                                });
                            });

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSerilog(dispose: true);
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                });

            await builder.RunConsoleAsync();

            Log.CloseAndFlush();
        }
    }
}