using MCApp.API.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace MCApp.API.ScheduledServices
{
    public partial class ScheduleTable
    {
        // Create<TProcess[, TProcessParam(=int)]>(schedule: , delay: , singleInstance: , iteration: ),
        public static ScheduleTable[] scheduleTable =  {
                Create<PollerProcess>(schedule: "schedule", delay: 35000, singleInstance: false, iteration: ProcessIteration.OnDemand),
                Create<TemplateProcess>(schedule: "schedule", delay: 15000, singleInstance: false, iteration: ProcessIteration.CustomSchedule)
        };
        // Ieder poller<Process> toevoegen als AddHostedService
        // Ieder Process toevoegen als ScopedService
        public static void AddScheduledServices(IServiceCollection services) {
            // dit wordt als tweede uitgevoerd vanuit startup.cs
            // handmatige actie:
            // Per Process een entry opnemen.
            AddScheduledService<PollerProcess>(services);
            AddScheduledService<TemplateProcess, int>(services);

        }
    }
}
