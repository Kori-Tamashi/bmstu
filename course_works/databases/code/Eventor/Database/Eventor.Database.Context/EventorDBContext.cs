using DotNetEnv;
using Eventor.Common.Enums;
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
    public virtual DbSet<UserDBModel> Users { get; set; }

    /// <summary>
    /// Таблица мероприятий
    /// </summary>
    public virtual DbSet<EventDBModel> Events { get; set; }

    /// <summary>
    /// Таблица дней мероприятий
    /// </summary>
    public virtual DbSet<DayDBModel> Days { get; set; }

    /// <summary>
    /// Таблица участников
    /// </summary>
    public virtual DbSet<PersonDBModel> Persons { get; set; }

    /// <summary>
    /// Таблица меню
    /// </summary>
    public virtual DbSet<MenuDBModel> Menu { get; set; }

    /// <summary>
    /// Таблица предметов
    /// </summary>
    public virtual DbSet<ItemDBModel> Items { get; set; }

    /// <summary>
    /// Таблица локаций
    /// </summary>
    public virtual DbSet<LocationDBModel> Locations { get; set; }

    /// <summary>
    /// Таблица отзывов
    /// </summary>
    public virtual DbSet<FeedbackDBModel> Feedbacks { get; set; }

    /// <summary>
    /// Таблица связи мероприятий и дней
    /// </summary>
    public virtual DbSet<EventDayDBModel> EventsDays { get; set; }

    /// <summary>
    /// Таблица связи меню и предметов
    /// </summary>
    public virtual DbSet<MenuItemsDBModel> MenuItems { get; set; }

    /// <summary>
    /// Таблица связи участников и дней
    /// </summary>
    public virtual DbSet<PersonDayDBModel> PersonsDays { get; set; }

    /// <summary>
    /// Таблица связи пользователей и мероприятий
    /// </summary>
    public virtual DbSet<UserEventDBModel> UsersEvents { get; set; }

    public EventorDBContext() { }

    public EventorDBContext(DbContextOptions<EventorDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Gender>("gender_enum");
        modelBuilder.HasPostgresEnum<UserRole>("user_role_enum");
        modelBuilder.HasPostgresEnum<ItemType>("item_type_enum");
        modelBuilder.HasPostgresEnum<PersonType>("person_type_enum");

        modelBuilder.Entity<EventDayDBModel>()
            .HasKey(ed => new { ed.EventId, ed.DayId });

        modelBuilder.Entity<MenuItemsDBModel>()
            .HasKey(mi => new { mi.MenuId, mi.ItemId });

        modelBuilder.Entity<PersonDayDBModel>()
            .HasKey(pd => new { pd.PersonId, pd.DayId });

        modelBuilder.Entity<UserEventDBModel>()
            .HasKey(ue => new { ue.UserId, ue.EventId });
    }

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

            optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<Gender>("gender_enum");
                npgsqlOptions.MapEnum<UserRole>("user_role_enum");
                npgsqlOptions.MapEnum<ItemType>("item_type_enum");
                npgsqlOptions.MapEnum<PersonType>("person_type_enum"); 
            });
        }
    }
}

