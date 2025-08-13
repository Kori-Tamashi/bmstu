using DotNetEnv;
using Eventor.Common.Enums;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Eventor.Database.Repositories;
using Eventor.GUI;
using Eventor.GUI.Controllers;
using Eventor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Net.NetworkInformation;

namespace Eventor;

internal static class Program
{
    const string _settingsFile = "dbsettings.env";

    static IServiceCollection _services;
    static ServiceProvider _servicesProvider;

    [STAThread]
    static void Main()
    {
        try
        {
            InitializeServices(UserRole.Guest, false);

            if (!CheckDatabaseConnection())
            {
                MessageBox.Show("Ошибка: не удалось подключиться к базе данных.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplicationRun();
        }
        catch
        {
            MessageBox.Show("Ошибка: произошла непредвиденная ошибка.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    static bool CheckDatabaseConnection()
    {
        try
        {
            var context = _servicesProvider.GetRequiredService<EventorDBContext>();
            return context.Database.CanConnect();
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    static void InitializeServices(UserRole userRole = UserRole.Guest, bool pooling = true)
    {
        _services = new ServiceCollection();
        ConfigureDI(_services, userRole, pooling);
        _servicesProvider = _services.BuildServiceProvider();
    }

    static void ConfigureDI(IServiceCollection services, UserRole userRole = UserRole.Guest, bool pooling = true)
    {
        ConfigureSettings();
        ConfigureContext(services, userRole, pooling);
        ConfigureLogging(services);
        ConfigureRepositories(services);
        ConfigureServices(services);
        ConfigureControllers(services);
        ConfigureForms(services);
    }

    static void ConfigureSettings()
    {
        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), _settingsFile));
    }

    static void ConfigureContext(IServiceCollection services, UserRole userRole = UserRole.Guest, bool pooling = true)
    {
        services.AddDbContext<EventorDBContext>(options =>
        {
            var connectionString = GetConnectionString(userRole, pooling);

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<Gender>("gender_enum");
                npgsqlOptions.MapEnum<UserRole>("user_role_enum");
                npgsqlOptions.MapEnum<ItemType>("item_type_enum");
                npgsqlOptions.MapEnum<PersonType>("person_type_enum");
            });
        },
        ServiceLifetime.Transient);
    }

    static void ConfigureLogging(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        });
    }

    static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IDayRepository, DayRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOauthService, OAuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IDayService, DayService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IEconomyService, EconomyService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
    }

    static void ConfigureControllers(IServiceCollection services)
    {
        services.AddTransient<MainWindowController>();
        services.AddTransient<EventFormController>();
        services.AddTransient<LoginFormController>();
        services.AddTransient<FeedbackCreateFormController>();
        services.AddTransient<FeedbacksFormController>();
        services.AddTransient<FeedbackFormController>();
        services.AddTransient<ParticipationFormController>();
        services.AddTransient<ParticipantsFormController>();
        services.AddTransient<DayFormController>();
        services.AddTransient<EventOrganizationFormController>();
        services.AddTransient<DayOrganizationFormController>();
        services.AddTransient<ItemCreateFormController>();
        services.AddTransient<LocationCreateFormController>();
        services.AddTransient<EventCreateFormController>();
    }

    static void ConfigureForms(IServiceCollection services)
    {
        services.AddTransient<MainWindow>(provider => new MainWindow());
        services.AddTransient<LoginForm>();
        services.AddTransient<EventForm>();
        services.AddTransient<FeedbackCreateForm>();
        services.AddTransient<FeedbacksForm>();
        services.AddTransient<FeedbackForm>();
        services.AddTransient<ParticipationForm>();
        services.AddTransient<ParticipantsForm>();
        services.AddTransient<DayForm>();
        services.AddTransient<EventOrganizationForm>();
        services.AddTransient<DayOrganizationForm>();
        services.AddTransient<ItemCreateForm>();
        services.AddTransient<LocationCreateForm>();
        services.AddTransient<EventCreateForm>();
    }

    private static string GetConnectionString(UserRole userRole, bool pooling = true)
    {
        return $"Host={Env.GetString("DB_HOST")};" +
               $"Port={Env.GetString("DB_PORT")};" +
               $"Database={Env.GetString("DB_NAME")};" +
               $"Username={GetDatabaseRole(userRole)};" +
               $"Password={GetDatabaseRolePassword(userRole)};" +
               $"Pooling={pooling}";
    }

    private static string GetDatabaseRole(UserRole role)
    {
        var roleString = role switch
        {
            UserRole.Admin => "admin",
            UserRole.User => "user_role",
            UserRole.Guest => "guest",
            _ => throw new ArgumentException($"Неизвестная роль: {role}")
        };

        return roleString ?? throw new ArgumentNullException($"Роль базы даных для роли {role} не определен");
    }

    private static string GetDatabaseRolePassword(UserRole role)
    {
        var password = role switch
        {
            UserRole.Admin => Env.GetString("DB_ADMIN_PASS"),
            UserRole.User => Env.GetString("DB_USER_PASS"),
            UserRole.Guest => Env.GetString("DB_GUEST_PASS"),
            _ => throw new ArgumentException($"Неизвестная роль: {role}")
        };

        return password ?? throw new ArgumentNullException($"Пароль для роли {role} не найден в .env");
    }

    static void ApplicationRun()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var loginForm = _servicesProvider.GetRequiredService<LoginForm>();

        if (loginForm.ShowDialog() == DialogResult.OK && loginForm.AuthenticatedUser != null)
        {
            InitializeServices(loginForm.AuthenticatedUser.Role);

            var controller = _servicesProvider.GetRequiredService<MainWindowController>();
            controller.CurrentUser = loginForm.AuthenticatedUser;

            var mainForm = _servicesProvider.GetRequiredService<MainWindow>();
            mainForm.SetController(controller);

            Application.Run(mainForm);
        }
        else
        {
            Application.Exit();
        }
    }
}
