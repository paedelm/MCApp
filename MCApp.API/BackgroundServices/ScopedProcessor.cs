using System;
using System.Threading;
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

        public override async Task<bool> ProcessAsync(TProcessParam param, CancellationToken stoppingToken) {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Console.WriteLine("in ScopedProcessor.ProcessAsync");
                bool isFinished;
                do {
                    isFinished = await ProcessInScope(scope.ServiceProvider, param, stoppingToken);
                } while (!isFinished && !stoppingToken.IsCancellationRequested);
                return true;
            }

        }
        public override bool CalculateDelay(out int delay, ScheduleTable schedule, int iteration) {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Console.WriteLine("in ScopedProcessor.CalculateDelay");
                return CalculateDelayInScope(scope.ServiceProvider, out delay, schedule, iteration);
            }
        }

        protected abstract Task<bool> ProcessInScope(IServiceProvider serviceProvider, TProcessParam param, CancellationToken stoppingToken);
        protected abstract bool CalculateDelayInScope(IServiceProvider serviceProvider, out int delay, ScheduleTable schedule, int iteration);
    }
}