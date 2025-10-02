using GameStore.Models.Entities;

namespace GameStore.Repositories;

public interface IGameRepository : IRepository<Game>
{
    // Game-specific operations
    Task<IReadOnlyList<Game>> GetActiveGamesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Game>> GetGamesByGenreAsync(int genreId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Game>> GetGamesByPlatformAsync(int platformId, CancellationToken cancellationToken = default);
    Task<Game?> GetGameWithDetailsAsync(int gameId, CancellationToken cancellationToken = default);

    // Advanced search with filtering and pagination
    Task<(IReadOnlyList<Game> Games, int TotalCount)> SearchGamesAsync(
        string? searchTerm = null,
        int? genreId = null,
        int? platformId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );
}
