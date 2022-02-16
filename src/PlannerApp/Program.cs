using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MudBlazor.Services;
using PlannerApp.Shared.Constants;
using System;
using System.Net.Http;
using Blazored.LocalStorage;

namespace PlannerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient(PlannerVariables.PLANNER_API, client =>
            {
                client.BaseAddress = new Uri(PlannerVariables.PLANNER_API_URL);
            }).AddHttpMessageHandler<AuthorizationMessageHandle>();

            builder.Services.AddTransient<AuthorizationMessageHandle>();
            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient(PlannerVariables.PLANNER_API));

            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();





            await builder.Build().RunAsync();
        }
    }
}
