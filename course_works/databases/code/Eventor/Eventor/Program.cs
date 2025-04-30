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
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var services = new ServiceCollection();
        ConfigureServices(services);

        using var provider = services.BuildServiceProvider();

        if (!CheckDatabaseConnection(provider))
        {
            MessageBox.Show("������ ����������� � ���� ������. ��������� ��������� �����������.",
                "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        using var loginForm = provider.GetRequiredService<LoginForm>();

        if (loginForm.ShowDialog() == DialogResult.OK && loginForm.AuthenticatedUser != null)
        {
            var mainForm = provider.GetRequiredService<MainWindow>();
            var controller = provider.GetRequiredService<MainWindowController>();

            controller.CurrentUser = loginForm.AuthenticatedUser;
            mainForm.SetController(controller);

            Application.Run(mainForm);
        }
        else
        {
            Application.Exit();
        }
    }

    static bool CheckDatabaseConnection(ServiceProvider provider)
    {
        try
        {
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EventorDBContext>();

            return context.Database.CanConnect();
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    static void ConfigureServices(IServiceCollection services)
    {
        string dir = Directory.GetCurrentDirectory();

        // �������� ���������� ���������
        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "dbsettings.env"));

        // ������������ ������ �����������
        var connectionString =
            $"Host={Env.GetString("DB_HOST")};" +
            $"Port={Env.GetString("DB_PORT")};" +
            $"Database={Env.GetString("DB_NAME")};" +
            $"Username={Env.GetString("DB_USER")};" +
            $"Password={Env.GetString("DB_PASSWORD")}";

        // ����������� ���������
        services.AddDbContext<EventorDBContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<Gender>("gender_enum");
                npgsqlOptions.MapEnum<UserRole>("user_role_enum");
                npgsqlOptions.MapEnum<ItemType>("item_type_enum");
                npgsqlOptions.MapEnum<PersonType>("person_type_enum");
            }));

        // ����������� �������� �����������
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole(); // ��� ������ ����� � �������
        });

        // ����������� 
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IDayRepository, DayRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        // ������� 
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOauthService, OAuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IDayService, DayService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IEconomyService, EconomyService>();
        services.AddScoped<IFeedbackService, FeedbackService>();

        // �����������
        services.AddTransient<MainWindowController>();
        services.AddTransient<EventFormController>();
        services.AddTransient<FeedbackFormController>();
        services.AddTransient<ParticipationFormController>();
        services.AddTransient<DayFormController>();

        // �����
        services.AddTransient<MainWindow>(provider => new MainWindow());
        services.AddTransient<LoginForm>();
        services.AddTransient<EventForm>();
        services.AddTransient<FeedbackForm>();
        services.AddTransient<ParticipationForm>();
        services.AddTransient<DayForm>();

    }
}
