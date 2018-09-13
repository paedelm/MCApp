using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MCApp.API.TypedHttpClients
{
    public class MCApiClient: TemplateHttpClient {
        public MCApiClient(HttpClient client): base(client) {}

    }
    public class TemplateHttpClient
    {
        public HttpClient Client { get;  }
        public TemplateHttpClient(HttpClient client)
        {
            Client = client;
        }
        public static void AddServices(IServiceCollection services) {
            services.AddHttpClient<MCApiClient>(client => {
                client.BaseAddress = new System.Uri("https://localhost:5001/api");
                client.DefaultRequestHeaders.Add("Accept", "Application/json");
                }
            );
        }
    }
}