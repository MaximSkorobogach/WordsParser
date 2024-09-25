using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WordsParser.Infrastructure.Database;
using WordsParser.Infrastructure.Repositories;

namespace WordsParser.ConsoleApp
{
    internal class Program
    {
        async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await InitializeDatabaseAsync(host);
        }

        private async Task InitializeDatabaseAsync(IHost host)
        {
            await host.Services.GetRequiredService<DatabaseInitializer>().InitializeDatabaseAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped(provider =>
                    {
                        var configuration = provider.GetRequiredService<IConfiguration>();
                        var connectionString = configuration.GetConnectionString("DefaultConnection")
                                               ?? throw new Exception("Не задана строка подключения к серверу ms sql");
                        return new DatabaseContext(connectionString);
                    });

                    services.AddSingleton<DatabaseInitializer>();
                    services.AddScoped<WordsRepository>(); 
                });
    }
}
