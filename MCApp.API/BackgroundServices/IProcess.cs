using System.Threading.Tasks;
using MCApp.API.ScheduledServices;

namespace MCApp.API.BackgroundServices
{
    public interface IProcess<TProcessParam>
    {
        Task ProcessAsync(TProcessParam param);
        bool CalculateDelay(out int delay, ScheduleTable schedule, int iteration);
    }
}