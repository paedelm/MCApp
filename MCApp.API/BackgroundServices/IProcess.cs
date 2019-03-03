using System.Threading;
using System.Threading.Tasks;
using MCApp.API.ScheduledServices;

namespace MCApp.API.BackgroundServices
{
    public interface IProcess<TProcessParam>
    {
        Task<bool> ProcessAsync(TProcessParam param, CancellationToken stoppingToken);
        bool CalculateDelay(out int delay, ScheduleTable schedule, int iteration);
    }
}