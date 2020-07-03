using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscBot.Modules
{
    [Group("game")]
    public class SampleModule : ModuleBase<SocketCommandContext>
    {
        private string channelToSay = "бот-говорит";
        readonly Random r = new Random();

        [Command("shuffle")]
        [Summary("Shuffle a numbers")]
        //public async Task SquareAsync([Summary("The number to square")] int num)
        public async Task SquareAsync()
        {
            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Name == "Мафия");

            int n = channel.Users.Count - 1;
            int[] perm = Enumerable.Range(1, n).ToArray();

            for (int z = n - 1; z >= 1; z--)
            {
                int j = r.Next(z + 1);

                (perm[j], perm[z]) = (perm[z], perm[j]);
            }


            await channel.Users.ToAsyncEnumerable().Where(ui => !ui.GuildPermissions.Administrator)
                .ForEachAsync(async (u, i) => await u.ModifyAsync(up =>
                {
                    if (u.Username == u.Nickname)
                    {
                        if (perm[i] < 10)
                            up.Nickname = $"0{perm[i]} {u.Username}";
                        else
                            up.Nickname = $"{perm[i]} {u.Username}";
                    }
                    else if (u.Nickname == null)
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
                }));
            await Context.Guild.TextChannels.First(c => c.Name == channelToSay).SendMessageAsync("Did it");
        }

        [Command("reset")]
        public async Task UserInfoAsync()
        {
            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Name == "Мафия");
            await channel.Users.ToAsyncEnumerable().ForEachAsync(async (u, i) => await u.ModifyAsync(up =>
            {
                if (!u.GuildPermissions.Administrator)
                {
                    up.Nickname = u.Nickname.Substring(2);
                }
            }));
            await Context.Guild.TextChannels.First(c => c.Name == channelToSay).SendMessageAsync("Reseted");
        }


        [Command("send")]
        public async Task Send()
        {
            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Name == "Мафия");

            int n = channel.Users.Count;
            string[] perm = new string[n];

            FillRolesPoMalenkomu(perm, n);

            for (int z = n - 1; z >= 1; z--)
            {
                int j = r.Next(z + 1);

                (perm[j], perm[z]) = (perm[z], perm[j]);
            }

            await channel.Users.ToAsyncEnumerable().ForEachAsync(async (u, i) =>
            {
                await u.SendMessageAsync(perm[i]);
            });

            await Context.Guild.TextChannels.First(c => c.Name == channelToSay).SendMessageAsync("Roles was sended");
        }

        [Command("send_roles")]
        public async Task SendRolesAsync()
        {
            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Name == "Мафия");

            int n = channel.Users.Count - 1;
            string[] perm = new string[n];

            FillRoles(perm, n);

            for (int z = n - 1; z >= 1; z--)
            {
                int j = r.Next(z + 1);

                string temp = perm[j];
                perm[j] = perm[z];
                perm[z] = temp;
            }



            await channel.Users.ToAsyncEnumerable().Where(ui => !ui.GuildPermissions.Administrator).ForEachAsync(async (u, i) =>
            {
                    await u.SendMessageAsync(perm[i]);
            });

            await Context.Guild.TextChannels.First(c => c.Name == channelToSay).SendMessageAsync("Roles was sended");
        }

        [Command("notify")]
        public async Task Notify()
        {
            await Context.Guild.Users.ToAsyncEnumerable().ForEachAsync(async u => await u.SendMessageAsync("ЗАХОДИ"));
            await Context.Guild.TextChannels.First(c => c.Name == channelToSay).SendMessageAsync("Notified");
        }

        private void FillRoles(string[] arr, int n)
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
