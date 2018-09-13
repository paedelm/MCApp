using System;
using System.Threading;
using System.Threading.Tasks;
using MCApp.API.ScheduledServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MCApp.API.BackgroundServices
{
    public class Poller<TProcess> : Poller<TProcess, int> where TProcess : IProcess<int>
    {
        public Poller(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) { }
    }
    public class Poller<TProcess, TProcessParam> : ScopedProcessor<TProcessParam> where TProcess : IProcess<TProcessParam>
    {

        public Poller(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            schedule = ScheduleTable.GetScheduleForProcess<TProcess, TProcessParam>();
            cmdQ = (BlockingCollection<TProcessParam>)schedule.CmdQue;
        }
        protected override bool CalculateDelayInScope(IServiceProvider serviceProvider, out int delay, ScheduleTable schedule, int iteration ) {
                var scopedProcessor = serviceProvider.GetService<TProcess>();
                    try
                    {
                        return scopedProcessor.CalculateDelay(out delay, schedule, iteration);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    delay = schedule.Delay;
                    return false;
        }

        protected override async Task ProcessInScope(IServiceProvider serviceProvider, TProcessParam param)
        {
            await Task.Run(async () =>
            {
                Console.WriteLine("in Poller.ProcessInScope 1");
                var scopedProcessor = serviceProvider.GetService<TProcess>();
                if (scopedProcessor == null)
                {
                    Console.WriteLine($"{typeof(TProcess).Name} is null");
                }
                else
                {
                    try
                    {
                        await scopedProcessor.ProcessAsync(param);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

    }
}