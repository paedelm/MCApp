using System;
using System.Threading.Tasks;
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

        protected override async Task ProcessAsync(TProcessParam param) {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Console.WriteLine("in ScopedProcessor.ProcessAsync with param");
                await ProcessInScope(scope.ServiceProvider, param);
            }

        }
        protected override async Task ProcessAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                Console.WriteLine("in ScopedProcessor.ProcessAsync");
                await ProcessInScope(scope.ServiceProvider);
            }
        }

        protected abstract Task ProcessInScope(IServiceProvider serviceProvider);
        protected abstract Task ProcessInScope(IServiceProvider serviceProvider, TProcessParam param);
    }
}