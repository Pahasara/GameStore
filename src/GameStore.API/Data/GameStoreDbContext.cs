using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options)
    {
    }

    // DbSets - Database tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Platform> Platforms => Set<Platform>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from the assembly

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameStoreDbContext).Assembly);

        // Add seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedTime = new DateTime(2025, 9, 30, 0, 0, 0, DateTimeKind.Utc);

        // Seed Genres
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action", Description = "Fast-paced games with combat and challenges", CreatedAt = seedTime },
            new Genre { Id = 2, Name = "Adventure", Description = "Story-driven exploration games", CreatedAt = seedTime },
            new Genre { Id = 3, Name = "RPG", Description = "Role-playing games with character progression", CreatedAt = seedTime },
            new Genre { Id = 4, Name = "Strategy", Description = "Tactical and strategic thinking games", CreatedAt = seedTime },
            new Genre { Id = 5, Name = "Sports", Description = "Athletic and sports simulation games", CreatedAt = seedTime }
        );

        // Seed Platforms
        modelBuilder.Entity<Platform>().HasData(
            new Platform { Id = 1, Name = "PC", Description = "Windows, Linux, and Mac computers", CreatedAt = seedTime },
            new Platform { Id = 2, Name = "PlayStation 5", Description = "Sony's latest gaming console", CreatedAt = seedTime },
            new Platform { Id = 3, Name = "Xbox Series X", Description = "Microsoft's latest gaming console", CreatedAt = seedTime },
            new Platform { Id = 4, Name = "Nintendo Switch", Description = "Nintendo's hybrid gaming console", CreatedAt = seedTime }
        );

        // Seed a sample game
        modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Title = "Dota 2",
                    Description = "A multiplayer online battle arena (MOBA) game developed by Valve.",
                    Price = 0.00m,
                    ReleaseDate = new DateTime(2013, 7, 9),
                    ImageUrl = "https://dota2.com/dota2.jpg",
                    IsActive = true,
                    GenreId = 4, // Strategy
                    PlatformId = 1, // PC
                    CreatedAt = seedTime,
                    UpdatedAt = seedTime
                }
        );
    }

    // Automatically update timestamps on save
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimeStamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimeStamps()
    {
        var now = DateTime.UtcNow;

        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified)
            .Where(e => e.Properties.Any(p => p.Metadata.Name == "UpdatedAt"));

        foreach (var entry in entries)
        {
            entry.Property("UpdatedAt").CurrentValue = now;
        }
    }
}
