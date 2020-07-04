using System;
using System.Linq;
using System.Threading.Tasks;
using DiscBot.Shared;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscBot.Modules
{
    public class MyBaseModule : ModuleBase<SocketCommandContext>
    {
        protected string channelToSay = ImportantStrings.ChannelToSay;
        protected Random r = new Random();

        protected SocketGuildChannel Channel;

        protected async Task DoSomethingForEverybodyExceptAdmin(Action<SocketGuildUser, int> action)
        {
            await Channel.Users.ToAsyncEnumerable()
                .Where(ui => !ui.GuildPermissions.Administrator)
                .ForEachAsync(action);
        }

        protected void InitChannel()
        {
             Channel ??= Context.Guild.Channels
                 .FirstOrDefault(c => c.Name == ImportantStrings.ChannelForGame);
        }

    }
}