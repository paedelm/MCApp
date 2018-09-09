using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MCApp.API.ScheduledServices;
using Microsoft.Extensions.Hosting;

namespace MCApp.API.BackgroundServices
{
    public abstract class MyBackgroundService : MyBackgroundService<int> { }
    public abstract class MyBackgroundService<TProcessParam> : BackgroundService
    {
        private Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts =
                                                       new CancellationTokenSource();
        protected int delay = 950000;
        public ScheduleTable schedule;
        public BlockingCollection<TProcessParam> cmdQ;
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                                                          cancellationToken));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //stoppingToken.Register(() =>
            //        _logger.LogDebug($" GracePeriod background task is stopping."));
            switch (schedule.Iteration)
            {
                case ScheduleTable.ProcessIteration.Repeating:
                    do
                    {
                        TProcessParam tdelay;
                        await ProcessAsync();
                        if (cmdQ.TryTake(out tdelay, schedule.Delay, stoppingToken))
                        {
                            Console.WriteLine($"gelukte TryTake tdelay={tdelay}");
                        }

                    }
                    while (!stoppingToken.IsCancellationRequested);
                    break;

                case ScheduleTable.ProcessIteration.OnDemand:
                    do
                    {
                        TProcessParam param;
                        if (cmdQ.TryTake(out param, schedule.Delay, stoppingToken))
                        {
                            Console.WriteLine($"gelukte TryTake param={param}");
                            await ProcessAsync(param);
                        }

                    }
                    while (!stoppingToken.IsCancellationRequested);
                    break;
                case ScheduleTable.ProcessIteration.Schedule:
                    // nog echt implementeren
                    do
                    {
                        TProcessParam param;
                        if (cmdQ.TryTake(out param, schedule.Delay, stoppingToken))
                        {
                            Console.WriteLine($"gelukte TryTake param={param}");
                            await ProcessAsync(param);
                        }

                    }
                    while (!stoppingToken.IsCancellationRequested);
                    break;
                default:
                    break;
            }
        }

        protected virtual Task ProcessAsync(TProcessParam param) { return Task.CompletedTask; }
        protected virtual Task ProcessAsync() { return Task.CompletedTask; }
    }
}