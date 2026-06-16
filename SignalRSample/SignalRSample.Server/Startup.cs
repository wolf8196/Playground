using JustMyPackages.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRSample.Api;
using SignalRSample.Server.Hubs;

namespace SignalRSample.Server
{
    public class Startup : IWebHostStartup
    {
        public void ConfigureHost(IHostBuilder host, IWebHostBuilder webHost)
        {
            host.ConfigureMyHost();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<Worker>();
            services.AddSignalR();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(x => x.MapHub<MyHub>($"/{Routes.MyHubRoute}"));
        }
    }
}
