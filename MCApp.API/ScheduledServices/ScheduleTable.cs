// using System;
using System;
using System.Collections.Concurrent;
using MCApp.API.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace MCApp.API.ScheduledServices
{
    public class ScheduleTable
    {
        public Type ProcessType { get; set; }
        public  string ProcessName  { get; set; }
        public string Schedule { get; set;}
        public int Delay { get; set; }
        public bool SingleInstance { get; set;}
        public int index { get; set;}
        public Object CmdQue { get; set; }
        public ProcessIteration Iteration { get; set; }
        // Voor iedere Process een nieuwe ScheduleTable opnemen
        // Create<TProcess, TProcessParam>()
        public enum ProcessIteration { OnDemand, Schedule, Repeating};

        public static ScheduleTable[] scheduleTable =  {
                Create<PollerProcess, int>(schedule: "schedule", delay: 15000, singleInstance: false, iteration: ProcessIteration.OnDemand)
        };
        // Ieder poller<Process> toevoegen als AddHostedService
        // Ieder Process toevoegen als ScopedService
        public static void AddScheduledServices(IServiceCollection services) {
            // Dit eerst doen met een foreach 
            foreach (var entry in scheduleTable) {
                services.AddScoped(entry.ProcessType);
            }
            // handmatige actie:
            // Per Process een entry opnemen.
            AddScheduledService<PollerProcess, int>(services);

        }
        public static void AddScheduledService<TProcess, TProcessParam>(IServiceCollection services) where TProcess: IProcess<TProcessParam> {
            services.AddHostedService<Poller<TProcess, TProcessParam>>();
        }
        public static ScheduleTable GetScheduleForProcess<TProcess, TProcessParam>() where TProcess: IProcess<TProcessParam> {
            var processName = typeof(TProcess).Name;
            foreach (var entry in scheduleTable) {
                if (entry.ProcessName.Equals(processName)) {
                    return entry;
                }            
            }
            return null;
        }
        public static ScheduleTable Create<TProcess, TProcessParam>(
            string schedule,
            bool singleInstance,
            int delay,
            ProcessIteration iteration) where TProcess: IProcess<TProcessParam> {
            BlockingCollection<TProcessParam> cmdq = new BlockingCollection<TProcessParam>(); 
            return new ScheduleTable {
                ProcessType = typeof(TProcess),
                ProcessName = typeof(TProcess).Name,
                Schedule = schedule,
                Delay = delay,
                SingleInstance = singleInstance,
                CmdQue = cmdq,
                Iteration = iteration
                };
        }
        public static void StartProcess(ScheduleTable entry, int delay) {
            StartProcess<int>(entry, delay);
        }
        public static void StartProcess<TProcessParam>(ScheduleTable entry, TProcessParam param) {
            BlockingCollection<TProcessParam> cmdQue = (BlockingCollection<TProcessParam>)entry.CmdQue;
            if (cmdQue.TryAdd(param, -1)) {
                Console.WriteLine("write on cmdQue succesfull!");
            } else {
                Console.WriteLine("write on cmdque failed");
            }
        }

        static ScheduleTable() {
            for (var i=0; i < scheduleTable.Length; i++) {
                scheduleTable[i].index = i;
            }
        }
    }
}