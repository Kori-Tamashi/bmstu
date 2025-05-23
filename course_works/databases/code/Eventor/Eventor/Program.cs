using DotNetEnv;
using Eventor.Common.Enums;
using Eventor.Database.Context;
using Eventor.Database.Core;
using Eventor.Database.Repositories;
using Eventor.GUI;
using Eventor.GUI.Controllers;
using Eventor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eventor;

internal static class Program
{
    static IServiceCollection _services;
    static ServiceProvider _servicesProvider;

    [STAThread]
    static void Main()
    {
        try
        {
            InitializeServices(UserRole.Guest);

            if (!CheckDatabaseConnection())
            {
                MessageBox.Show("������: �� ������� ������������ � ���� ������.",
                    "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplicationRun();
        }
        catch
        {
            MessageBox.Show("������: ��������� �������������� ������.",
                    "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    static void InitializeServices(UserRole userRole = UserRole.Guest)
    {
        _services = new ServiceCollection();
        ConfigureDI(_services, userRole);
        _servicesProvider = _services.BuildServiceProvider();
    }

    static void ConfigureDI(IServiceCollection services, UserRole userRole = UserRole.Guest)
    {
        ConfigureContext(services, userRole);
        ConfigureLogging(services);
        ConfigureRepositories(services);
        ConfigureServices(services);
        ConfigureControllers(services);
        ConfigureForms(services);
    }

    static void ConfigureContext(IServiceCollection services, UserRole userRole = UserRole.Guest)
    {
        services.AddDbContext<EventorDBContext>(options =>
        {
            Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "dbsettings.env"));

            var connectionString =
                $"Host={Env.GetString("DB_HOST")};" +
                $"Port={Env.GetString("DB_PORT")};" +
                $"Database={Env.GetString("DB_NAME")};" +
                $"Username={GetDatabaseRole(userRole)};" + 
                $"Password={GetDatabaseRolePassword(userRole)};";

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<Gender>("gender_enum");
                npgsqlOptions.MapEnum<UserRole>("user_role_enum");
                npgsqlOptions.MapEnum<ItemType>("item_type_enum");
                npgsqlOptions.MapEnum<PersonType>("person_type_enum");
            });
        });
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

    private static string GetDatabaseRole(UserRole role)
    {
        var roleString = role switch
        {
            UserRole.Admin => "admin",
            UserRole.User => "user_role",
            UserRole.Guest => "guest",
            _ => throw new ArgumentException($"����������� ����: {role}")
        };

        return roleString ?? throw new ArgumentNullException($"���� ���� ����� ��� ���� {role} �� ���������");
    }

    private static string GetDatabaseRolePassword(UserRole role)
    {
        var password = role switch
        {
            UserRole.Admin => Env.GetString("DB_ADMIN_PASS"),
            UserRole.User => Env.GetString("DB_USER_PASS"),
            UserRole.Guest => Env.GetString("DB_GUEST_PASS"),
            _ => throw new ArgumentException($"����������� ����: {role}")
        };

        return password ?? throw new ArgumentNullException($"������ ��� ���� {role} �� ������ � .env");
    }

    static void ApplicationRun()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var loginForm = _servicesProvider.GetRequiredService<LoginForm>();

        if (loginForm.ShowDialog() == DialogResult.OK && loginForm.AuthenticatedUser != null)
        {
            var userRole = loginForm.AuthenticatedUser.Role;
            InitializeServices(userRole);

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
