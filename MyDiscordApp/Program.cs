using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyDiscordApp.Services;
using System;
using System.Threading.Tasks;

namespace MyDiscordApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            await Task.Delay(-1);
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config.AddJsonFile("appsetting.json");
                })
                .ConfigureLogging(log => 
                {

                })
                .ConfigureServices((hostcontent, service) =>
                {
                    service.AddHostedService<CommandHandler>()
                           .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                           {
                               GatewayIntents = GatewayIntents.Guilds
                           }));
                });
        }
    }
}
