using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;

namespace MarketIntelligency.Application.SA0001
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddSimpleConsole(options =>
                    {
                        options.ColorBehavior = LoggerColorBehavior.Enabled;
                        options.SingleLine = true;
                        options.TimestampFormat = " hh:mm:ss.ffffff | ";
                    });
                    logging.AddFilter("System.Net.Http.HttpClient.Default.LogicalHandler", LogLevel.None);
                    logging.AddFilter("System.Net.Http.HttpClient.Default.ClientHandler", LogLevel.None);
                })
                .UseSerilog((ctx, provider, loggeConfig) =>
                {
                    loggeConfig.ReadFrom.Configuration(ctx.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq("http://host.docker.internal:5341");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
