using System.Linq;
using Discord.Commands;
using System.Threading.Tasks;

namespace DiscBot.Modules
{
    public class InfoModule : MyBaseModule
    {
        [Command("say")]
        [Summary("Echoes a message")]
        public async Task SayAsync([Remainder] [Summary("The text to echo")]
            string echo)
        {
            InitChannel();
            
            await Context.Guild.TextChannels
                .First(c => c.Name == channelToSay)
                .SendMessageAsync(echo);
        }
    }
}
