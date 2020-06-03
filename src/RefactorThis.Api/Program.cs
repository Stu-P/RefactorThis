using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RefactorThis.Api.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace RefactorThis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationVersion", ApplicationVersion.Value)
                .Enrich.WithProperty("Hostname", Dns.GetHostName())
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.Seq("http://refactorlog:5341")
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://*:5000")
                    .UseStartup<Startup>();
                });
    }
}