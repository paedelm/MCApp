using System;
using System.Threading.Tasks;
using MCApp.API.Data;
using MCApp.API.Helpers;
using MCApp.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MCApp.API.BackgroundServices
{
    public class PollerProcess : IProcess<int>
    {
        private readonly ILogger<PollerProcess> _logger;
        private readonly IMicroCreditRepository _repo;

        public PollerProcess(ILogger<PollerProcess> logger, IMicroCreditRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        public async Task ProcessAsync() {
            await ProcessAsync(5000);
        }
        public async Task ProcessAsync(int delay)
        {
            var user = await _repo.GetUser("paedelm");
            Console.WriteLine($"userid: {user.Id} knownAs:{user.KnownAs} lastactive:{user.LastActive}");
            PagedList<Mutation> pg = await _repo.GetMutationsForUserAccount(new Helpers.MutationParams { UserId = 1, AccountId = 1, PageNumber = 1, PageSize = 20 });
            Console.WriteLine($"PageSize={pg.PageSize} en Count={pg.Count}");
            foreach (var mut in pg)
            {
                Console.WriteLine($"{mut.Account.Accountname} - {mut.Amount} - {mut.Balance} - {mut.Created}");
            }
            string json = JsonConvert.SerializeObject(value: pg, formatting: Formatting.Indented, settings: new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            // Console.WriteLine($"json={json}");
            _logger.LogInformation("PollerProcess via logger");
            await Task.Run(() =>
            {
                //    logger.LogInformation("In de poller Process procedure"));
                Console.WriteLine($"In de poller Process procedure {delay}");
            });
        }

    }
}