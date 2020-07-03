using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscBot.Services;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<LoggingService>();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 100
            }));

            services.AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async
            }));

            services.AddSingleton<CommandHandler>();
            services.AddSingleton<StartupService>();
            services.AddSingleton<LoggingService>();
            services.AddSingleton<Random>();
            services.AddSingleton(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
