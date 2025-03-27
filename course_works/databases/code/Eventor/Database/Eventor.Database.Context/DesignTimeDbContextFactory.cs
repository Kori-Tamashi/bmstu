using DotNetEnv;
using Eventor.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EventorDBContext>
{
    public EventorDBContext CreateDbContext(string[] args)
    {
        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), "dbsettings.env"));

        var connectionString =
            $"Host={Env.GetString("DB_HOST")};" +
            $"Port={Env.GetString("DB_PORT")};" +
            $"Database={Env.GetString("DB_NAME")};" +
            $"Username={Env.GetString("DB_USER")};" +
            $"Password={Env.GetString("DB_PASSWORD")}";

        var optionsBuilder = new DbContextOptionsBuilder<EventorDBContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new EventorDBContext(optionsBuilder.Options);
    }
}