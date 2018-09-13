using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MCApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);
            Console.WriteLine($"urls={host.GetSetting(WebHostDefaults.PreferHostingUrlsKey)}");
            var buildedHost = host.Build();
            // var urls = buildedHost.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            // Console.WriteLine($"aantal urls={urls.Count}");
            // foreach (var url in urls) {
            //     Console.WriteLine($"url={url}");
            // }
            // foreach (var feature in buildedHost.ServerFeatures) {
            //     Console.WriteLine($"feature<key,value>=<{feature.Key},{feature.Value}>");
            // }
            buildedHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}