using System;
using System.Threading.Tasks;
using MCApp.API.BackgroundServices;

namespace MCApp.API.ScheduledServices
{
    public class TemplateProcess : TemplateProcess<int>
    {
        public TemplateProcess()
        {
            defValue = 1;
        }
    }
    public class TemplateProcess<TProcessParam> : IProcess<TProcessParam>, IPollerProcess<TProcessParam>
    {
        //
        public TProcessParam defValue;
        public async virtual Task ProcessAsync(TProcessParam param)
        {
            await Task.Run(() => Console.WriteLine($"{this.GetType().Name}: ProcessAsync(TProcessParam) not implemented"));
        }
        public virtual bool CalculateDelay(out int delay, ScheduleTable schedule, int iteration)
        {
            delay = -1; // maar 1 keer uitvoeren
            Console.WriteLine($"{schedule.ProcessName}:Override CalculateDelay to implement own schedule logic");
            return false;
        }

    }
}