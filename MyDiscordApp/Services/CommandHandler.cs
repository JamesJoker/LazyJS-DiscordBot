using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyDiscordApp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyDiscordApp.Services
{
    public class CommandHandler : IHostedService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;

        public CommandHandler(IConfiguration config, IServiceProvider services)
        {
            _config = config;
            _services = services;
            _commands = new CommandService();
            _discord = new DiscordSocketClient();
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _discord.LoginAsync(Discord.TokenType.Bot, _config["Token"]);
            await _discord.StartAsync();


            _discord.Ready += OnReady;
            _discord.MessageReceived += OnReceived;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Shut down!");
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            Console.WriteLine($"{_discord.CurrentUser.Username} is login!");
            return Task.CompletedTask;
        }

        private async Task OnReceived(SocketMessage arg)
        {
            if(!(arg is SocketUserMessage message)) return;

            var argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos)) return;
            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}
