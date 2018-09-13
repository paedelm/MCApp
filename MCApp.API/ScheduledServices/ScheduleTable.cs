// using System;
using System;
using System.Collections.Concurrent;
using MCApp.API.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace MCApp.API.ScheduledServices
{
    public partial class ScheduleTable
    {
        public Type ProcessType { get; set; }
        public  string ProcessName  { get; set; }
        public string Schedule { get; set;}
        public int Delay { get; set; }
        public bool SingleInstance { get; set;}
        public int index { get; set;}
        public Object CmdQue { get; set; }
        public ProcessIteration Iteration { get; set; }
        // Voor iedere Process een nieuwe ScheduleTable opnemen zie ScheduleTableServices.cs
        // Create<TProcess, TProcessParam>()
        public enum ProcessIteration { OnDemand, CustomSchedule, Repeating};

        public static void AddScopedServices(IServiceCollection services) {
            // dit wordt als eerste uitgevoerd vanuit startup.cs
            // geen handmatige actie 
            foreach (var entry in scheduleTable) {
                services.AddScoped(entry.ProcessType);
            }
        }
        public static void AddScheduledService<TProcess>(IServiceCollection services) where TProcess: IProcess<int> {
            AddScheduledService<TProcess, int>(services);
        }
        public static void AddScheduledService<TProcess, TProcessParam>(IServiceCollection services) where TProcess: IProcess<TProcessParam> {
            services.AddHostedService<Poller<TProcess, TProcessParam>>();
            var schedule = ScheduleTable.GetScheduleForProcess<TProcess, TProcessParam>();
            Console.WriteLine($"Registered {schedule.Iteration} Process: {schedule.ProcessName}");
        }
        public static ScheduleTable GetScheduleForProcess<TProcess>() where TProcess: IProcess<int> {
            return GetScheduleForProcess<TProcess, int>();
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
        public static ScheduleTable Create<TProcess>(
            string schedule,
            bool singleInstance,
            int delay,
            ProcessIteration iteration) where TProcess: IProcess<int> {
                return Create<TProcess, int>(
                    schedule: schedule,
                    delay: delay,
                    singleInstance: singleInstance,
                    iteration: iteration
                );
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