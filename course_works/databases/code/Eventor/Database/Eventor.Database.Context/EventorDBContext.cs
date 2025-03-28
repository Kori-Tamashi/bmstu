using DotNetEnv;
using Eventor.Database.Models;
using Microsoft.EntityFrameworkCore;
namespace Eventor.Database.Context;

/// <summary>
/// Контекст базы данных
/// </summary>
public class EventorDBContext : DbContext
{
    /// <summary>
    /// Таблица пользователй
    /// </summary>
    public DbSet<UserDBModel> Users { get; set; }

    /// <summary>
    /// Таблица мероприятий
    /// </summary>
    public DbSet<EventDBModel> Events { get; set; }

    /// <summary>
    /// Таблица дней мероприятий
    /// </summary>
    public DbSet<DayDBModel> Days { get; set; }

    /// <summary>
    /// Таблица участников
    /// </summary>
    public DbSet<PersonDBModel> Persons { get; set; }

    /// <summary>
    /// Таблица меню
    /// </summary>
    public DbSet<MenuDBModel> Menu { get; set; }

    /// <summary>
    /// Таблица предметов
    /// </summary>
    public DbSet<ItemDBModel> Items { get; set; }

    /// <summary>
    /// Таблица локаций
    /// </summary>
    public DbSet<LocationDBModel> Locations { get; set; }

    /// <summary>
    /// Таблица отзывов
    /// </summary>
    public DbSet<FeedbackDBModel> Feedbacks { get; set; }

    public EventorDBContext(DbContextOptions<EventorDBContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "dbsettings.env"));

            var connectionString =
                $"Host={Env.GetString("DB_HOST")};" +
                $"Port={Env.GetString("DB_PORT")};" +
                $"Database={Env.GetString("DB_NAME")};" +
                $"Username={Env.GetString("DB_USER")};" +
                $"Password={Env.GetString("DB_PASSWORD")}";

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}

