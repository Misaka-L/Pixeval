using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Pixeval.UI;

namespace Pixeval
{
    class Program
    {
        [STAThread]
        private static async Task Main()
        {
            var host = CreateHostBuilder().Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var app = services.GetRequiredService<App>();
                    app.Run();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred.");
                }
            }
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<CoreWebView2>();
                    services.AddSingleton<SignIn>();
                    services.AddSingleton<App>();
                })
                .ConfigureLogging(config =>
                {
                    
                });
    }
}
