using DotNetEnv;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Eventor.Database.Repositories;
using Eventor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace Eventor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);

            using var provider = services.BuildServiceProvider();
            Application.Run(provider.GetRequiredService<MainWindow>());
        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Загрузка переменных окружения
            Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "dbsettings.env"));

            // Формирование строки подключения
            var connectionString =
                $"Host={Env.GetString("DB_HOST")};" +
                $"Port={Env.GetString("DB_PORT")};" +
                $"Database={Env.GetString("DB_NAME")};" +
                $"Username={Env.GetString("DB_USER")};" +
                $"Password={Env.GetString("DB_PASSWORD")}";

            // Регистрация контекста
            services.AddDbContext<EventorDBContext>(options =>
                options.UseNpgsql(connectionString));

            // Регистрация сервисов логирования
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole(); // Для вывода логов в консоль
            });

            // Репозитории 
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IDayRepository, DayRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Сервисы 
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOauthService, OAuthService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IDayService, DayService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IEconomyService, EconomyService>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            // Формы
            services.AddSingleton<MainWindow>();
        }
    }
}