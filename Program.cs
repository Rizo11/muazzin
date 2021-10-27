using System;
using System.Threading.Tasks;
using bot.Entity;
using bot.HttpClients;
using bot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace bot
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        static Task Main(string[] args)
            => CreateHostBuilder(args)
                .Build()
                .RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices(Configure)
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    Configuration = configuration.Build();
                }); 

        private static void Configure(HostBuilderContext context, IServiceCollection services)
        {
            services.AddDbContext<BotDbContext>(options => 
                {
                    options.UseSqlite(Configuration.GetConnectionString("BotConnection  "));
                }
            );

            services.AddMemoryCache();
            services.AddTransient<ICacheService, PrayerTimeCacheService>();
            services.AddSingleton<TelegramBotClient>(b => new TelegramBotClient("2080106349:AAFGYis_5wg8vkDNWVIdbN_sR893gLFIHJw"));
            services.AddHostedService<Bot>();
            services.AddTransient<IStorageService, InternalStorageService>();
            services.AddTransient<Handlers>();
            services.AddHttpClient<IPrayerTimeClient, AladhanClient>
            (client => 
            {
                client.BaseAddress = new Uri("http://api.aladhan.com/v1");
            });
 
        }
    }
}