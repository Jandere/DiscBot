using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using DiscBot.Shared;

namespace DiscBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        public StartupService(IServiceProvider service, DiscordSocketClient discord, CommandService commands)
        {
            this._commands = commands;
            this._provider = service;
            this._discord = discord;
        }

        public async Task StartAsync()
        {
            string discordToken = ImportantStrings.BotToken;

            await _discord.LoginAsync(Discord.TokenType.Bot, discordToken);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
