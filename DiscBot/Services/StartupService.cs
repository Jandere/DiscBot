using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;

        public StartupService(IServiceProvider service, DiscordSocketClient discord, CommandService commands)
        {
            this.commands = commands;
            this.provider = service;
            this.discord = discord;
        }

        public async Task StartAsync()
        {
            string discordToken = "NzI2Mzg2OTc2NjQ2ODg5NTAz.Xvc3oA.Xw65rpH3zlFUoBVkVYiul2zwARU";

            await discord.LoginAsync(Discord.TokenType.Bot, discordToken);
            await discord.StartAsync();

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
        }
    }
}
