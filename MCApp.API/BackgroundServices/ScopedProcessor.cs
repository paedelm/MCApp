using System;
using System.Threading.Tasks;
using MCApp.API.ScheduledServices;
using Microsoft.Extensions.DependencyInjection;

namespace MCApp.API.BackgroundServices
{
    public abstract class ScopedProcessor<TProcessParam> : MyBackgroundService<TProcessParam>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task ProcessAsync(TProcessParam param) {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Console.WriteLine("in ScopedProcessor.ProcessAsync");
                await ProcessInScope(scope.ServiceProvider, param);
            }

        }
        public override bool CalculateDelay(out int delay, ScheduleTable schedule, int iteration) {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Console.WriteLine("in ScopedProcessor.CalculateDelay");
                return CalculateDelayInScope(scope.ServiceProvider, out delay, schedule, iteration);
            }
        }

        protected abstract Task ProcessInScope(IServiceProvider serviceProvider, TProcessParam param);
        protected abstract bool CalculateDelayInScope(IServiceProvider serviceProvider, out int delay, ScheduleTable schedule, int iteration);
    }
}