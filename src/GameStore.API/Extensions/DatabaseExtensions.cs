using GameStore.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<GameStoreDbContext>(options =>
        {
            options.UseSqlite(connectionString);

            // Enable detailed errors in development
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }

    public static async Task<WebApplication> ApplyMigrationsAsync(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameStoreDbContext>();
            await context.Database.MigrateAsync();
            return app;
        }
        catch (Exception ex)
        {
            // Log the error (will add logging later)
            throw new InvalidOperationException("Failed to apply database migrations", ex);
        }
    }
}
