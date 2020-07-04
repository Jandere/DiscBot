using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscBot.Modules
{
    [Group("test")]
    public class TestModule : MyBaseModule
    {
        [Command("send_roles")]
        public async Task Send()
        {
            InitChannel();
            
            if (Channel.Users.Count < 2)
                return;
            
            int n = Channel.Users.Count;
            string[] perm = new string[n];

            FillRolesPoMalenkomu(perm, n);

            for (int z = n - 1; z >= 1; z--)
            {
                int j = r.Next(z + 1);

                (perm[j], perm[z]) = (perm[z], perm[j]);
            }

            await Channel.Users.ToAsyncEnumerable().ForEachAsync(async (u, i) =>
            {
                await u.SendMessageAsync(perm[i]);
            });

            await Context.Guild.TextChannels
                .First(c => c.Name == channelToSay)
                .SendMessageAsync("Роли были отправлены");
        }
        
        
        private void FillRolesPoMalenkomu(string[] arr, int n)
        {
            Dictionary<string, int> roles = new Dictionary<string, int>(n)
            {
                { "мафия", 1 },
                { "ком", 1 },
                { "мирный житель", n - 2}
            };
            int i = 0;
            foreach (var role in roles)
            {
                for (int j = 0; j < role.Value; j++)
                {
                    arr[i] = role.Key;
                    i++;
                }
            }
        }
    }
}