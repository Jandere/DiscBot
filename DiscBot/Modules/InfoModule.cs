using System.Linq;
using Discord.Commands;
using System.Threading.Tasks;

namespace DiscBot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message")]
        public async Task SayAsync([Remainder] [Summary("The text to echo")]
            string echo)
        {
            await Context.Guild.TextChannels.First(c => c.Name == "бот-говорит").SendMessageAsync(echo);

        }
    }
}
