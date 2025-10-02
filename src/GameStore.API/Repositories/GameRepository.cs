using GameStore.Data;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories;

public class GameRepository : Repository<Game>, IGameRepository
{
    public GameRepository(GameStoreDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Game>> GetActiveGamesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.IsActive)
            .Include(g => g.Genre)
            .Include(g => g.Platform)
            .OrderBy(g => g.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Game>> GetGamesByGenreAsync(int genreId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.GenreId == genreId && g.IsActive)
            .Include(g => g.Genre)
            .Include(g => g.Platform)
            .OrderBy(g => g.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Game>> GetGamesByPlatformAsync(int platformId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(g => g.PlatformId == platformId && g.IsActive)
            .Include(g => g.Genre)
            .Include(g => g.Platform)
            .OrderBy(g => g.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Game?> GetGameWithDetailsAsync(int gameId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Genre)
            .Include(g => g.Platform)
            .Include(g => g.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(g => g.Id == gameId, cancellationToken);
    }

    public async Task<(IReadOnlyList<Game> Games, int TotalCount)> SearchGamesAsync(
        string? searchTerm = null,
        int? genreId = null,
        int? platformId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Dynamic query construction
        IQueryable<Game> query = _dbSet
            .Include(g => g.Genre)
            .Include(g => g.Platform)
            .Where(g => g.IsActive);

        // Conditional query building
        // Text seach in Title and Description
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(g =>
                g.Title.Contains(searchTerm) ||
                (g.Description != null && g.Description.Contains(searchTerm)));
        }

        // Apply other filters
        if (genreId.HasValue)
            query = query.Where(g => g.GenreId == genreId);
        if (platformId.HasValue)
            query = query.Where(g => g.PlatformId == platformId);
        if (minPrice.HasValue)
            query = query.Where(g => g.Price >= minPrice);
        if (maxPrice.HasValue)
            query = query.Where(g => g.Price <= maxPrice);

        var totalCount = await query.CountAsync(cancellationToken);


        var games = await query
            .OrderBy(g => g.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (games, totalCount);
    }
}
