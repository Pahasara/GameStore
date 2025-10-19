using GameStore.Common;
using GameStore.DTOs.Requests;
using GameStore.DTOs.Responses;

namespace GameStore.Services;

public interface IGameService
{
    Task<Result<PagedResponse<GameSummaryDto>>> SearchGamesAsync(
        string? searchTerm = null,
        int? genreId = null,
        int? platformId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    );

    Task<Result<GameDto>> GetGameByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<GameSummaryDto>>> GetActiveGamesAsync(CancellationToken cancellationToken = default);

    Task<Result<GameDto>> CreateGameAsync(CreateGameRequest request, CancellationToken cancellationToken = default);

    Task<Result<GameDto>> UpdateGameAsync(int id, UpdateGameRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteGameAsync(int id, CancellationToken cancellationToken = default);
}
