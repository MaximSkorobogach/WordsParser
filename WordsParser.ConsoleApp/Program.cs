using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WordsParser.Infrastructure.Database.Interfaces;
using WordsParser.Infrastructure.DI;
using WordsParser.Infrastructure.Handlers.Interfaces;

namespace WordsParser.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder().Build();

            await InitializeDatabaseAsync(host);

            await host.Services.GetRequiredService<IEndlessWordParserHandler>().TryStartHandlingAsync();
        }


        private static async Task InitializeDatabaseAsync(IHost host)
        {
            await host.Services.GetRequiredService<IDatabaseInitializer>().InitializeDatabaseAsync();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    services.AddInfrastructure(configuration);
                });
    }
}
