using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MCApp.API.ScheduledServices;
using Microsoft.Extensions.Hosting;

namespace MCApp.API.BackgroundServices
{
    public abstract class MyBackgroundService : MyBackgroundService<int>
    {
        public MyBackgroundService()
        {
            defValue = 1;
        }
    }
    public abstract class MyBackgroundService<TProcessParam> : BackgroundService, IPollerProcess<TProcessParam>
    {
        protected TProcessParam defValue;
        public ScheduleTable schedule;
        public BlockingCollection<TProcessParam> cmdQ;
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TProcessParam param;
            switch (schedule.Iteration)
            {
                case ScheduleTable.ProcessIteration.Repeating:
                    try
                    {
                        do
                        {
                            await ProcessAsync(defValue);
                            if (cmdQ.TryTake(item: out param, millisecondsTimeout: schedule.Delay, cancellationToken: stoppingToken))
                            {
                                Console.WriteLine($"gelukte TryTake param={param}");
                            }

                        }
                        while (!stoppingToken.IsCancellationRequested);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Process {schedule.ProcessName} OperationCanceledException");
                    }
                    break;

                case ScheduleTable.ProcessIteration.OnDemand:
                    try
                    {
                        do
                        {
                            if (cmdQ.TryTake(item: out param, millisecondsTimeout: -1, cancellationToken: stoppingToken))
                            {
                                Console.WriteLine($"gelukte TryTake param={param}");
                                await ProcessAsync(param);
                            }

                        }
                        while (!stoppingToken.IsCancellationRequested);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Process {schedule.ProcessName} OperationCanceledException");
                    }
                    break;
                case ScheduleTable.ProcessIteration.CustomSchedule:
                    // Custom betekent vragen om de delay en of er uitgevoerd moet worden na timeout
                    // let op: bij trytake succes wordt er wel uitgevoerd.
                    // Crontab kan via CalculateDelay worden geimplementeerd
                    int iteration = 0;
                    try
                    {
                        do
                        {
                            int delay;
                            bool xqtAfterTimeout = CalculateDelay(out delay, schedule, iteration);
                            Console.WriteLine($"xqtAfterTimeout, delay={xqtAfterTimeout},{delay}");
                            if (cmdQ.TryTake(item: out param, millisecondsTimeout: delay, cancellationToken: stoppingToken))
                            {
                                Console.WriteLine($"gelukte TryTake param={param}");
                                await ProcessAsync(param);
                            }
                            else if (xqtAfterTimeout)
                            {
                                Console.WriteLine($"Execute because of Custom procedure");
                                await ProcessAsync(param);
                            }
                            iteration++;
                        }
                        while (!stoppingToken.IsCancellationRequested);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Process {schedule.ProcessName} OperationCanceledException");
                    }
                    break;
            }
            Console.WriteLine($"Process {schedule.ProcessName} stopped");
        }

        public abstract Task ProcessAsync(TProcessParam param);
        public abstract bool CalculateDelay(out int delay, ScheduleTable schedule, int iteration);
    }
}