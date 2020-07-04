using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscBot.Modules
{
    [Group("game")]
    public class GameModule : MyBaseModule
    {
        [Command("shuffle")]
        [Summary("Раздает номера перед никами")]
        //public async Task SquareAsync([Summary("The number to square")] int num)
        public async Task ShuffleAsync()
        {
            InitChannel();

            if (Channel.Users.Count < 2)
                return;
            
            int n = Channel.Users.Count - 1;
            List<int> perm = Enumerable.Range(1, n).ToList();

            ShuffleValues(perm, n);
            
            await DoSomethingForEverybodyExceptAdmin(async (u, i) =>
            {
                await u.ModifyAsync(up =>
                {
                    if (u.Nickname == null)
                    {
                        if (perm[i] < 10)
                            up.Nickname = $"0{perm[i]} {u.Username}";
                        else
                            up.Nickname = $"{perm[i]} {u.Username}";
                    }
                    else
                    {
                        if (perm[i] < 10)
                            up.Nickname = $"0{perm[i]} {u.Nickname}";
                        else
                            up.Nickname = $"{perm[i]} {u.Nickname}";
                    }
                });
            });
            
            await Context.Guild.TextChannels.First(c => c.Name == channelToSay).SendMessageAsync("Did it");
        }

        [Command("reset")]
        [Summary("Удаляет номера перед никами")]
        public async Task ResetAsync()
        {
            InitChannel();
            
            if (Channel.Users.Count < 2)
                return;
            
            await DoSomethingForEverybodyExceptAdmin(async (u, i) =>
            {
                await u.ModifyAsync(up =>
                {
                    up.Nickname = u.Nickname.Substring(2);
                });
            });
            
            await Context.Guild.TextChannels
                .First(c => c.Name == channelToSay)
                .SendMessageAsync("Удалил номера");
        }

        [Command("send_roles")]
        [Summary("Отправляет роли игрокам в лс")]
        public async Task SendRolesAsync()
        {
            InitChannel();
            
            if (Channel.Users.Count < 2)
                return;
            
            int n = Channel.Users.Count - 1;
            List<string> perm = new List<string>(n);

            FillRoles(perm, n);
            ShuffleValues(perm, n);

            await DoSomethingForEverybodyExceptAdmin(async (u, i) =>
            {
                await u.SendMessageAsync(perm[i]);
            });

            
            await Context.Guild.TextChannels
                .First(c => c.Name == channelToSay)
                .SendMessageAsync("Отправил всем роли");
        }

        [Command("notify")]
        [Summary("Созвать людей поиграть в мафию")]
        public async Task Notify()
        {
            InitChannel();
            
            if (Channel.Users.Count < 2)
                return;
            
            await DoSomethingForEverybodyExceptAdmin(async (u, i) =>
            {
                await u.SendMessageAsync("ЗАХОДИ");
            });
            
            await Context.Guild.TextChannels
                .First(c => c.Name == channelToSay)
                .SendMessageAsync("Всех созвал");
        }

        private void FillRoles(List<string> arr, int n)
        {
            Dictionary<string, int> roles = new Dictionary<string, int>(n)
            {
                { "мафия", 2 },
                { "коп", 1 },
                { "доктор", 1 },
                { "мирный житель", n - 4 }
            };
            if (n >= 10 && n < 12)
            {
                roles.Add("дон", 1);
                roles["мирный житель"] -= 1;
            }
            else if (n >= 12)
            {
                roles.Add("ночная бабочка", 1);
                roles["мирный житель"] -= 1;
            }
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

        private void ShuffleValues<T>(List<T> values, int n)
        {
            for (int z = n - 1; z >= 1; z--)
            {
                int j = r.Next(z + 1);

                (values[j], values[z]) = (values[z], values[j]);
            }
        }

        
    }
}
