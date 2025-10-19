using GameStore.Common;
using GameStore.DTOs.Requests;
using GameStore.DTOs.Responses;
using GameStore.Extensions;
using GameStore.Repositories;

namespace GameStore.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GameService> _logger;

    public GameService(IUnitOfWork unitOfWork, ILogger<GameService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PagedResponse<GameSummaryDto>>> SearchGamesAsync(
        string? searchTerm,
        int? genreId,
        int? platformId,
        decimal? minPrice,
        decimal? maxPrice,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching games: term{Term}, genre={Genre}, page={Page}", searchTerm, genreId, pageNumber);

        try
        {
            // Input validation
            if (pageNumber < 1)
                return Result<PagedResponse<GameSummaryDto>>.Failure("Page number must be at least 1", ErrorType.Validation);

            if (pageSize < 1 || pageSize > 100)
                return Result<PagedResponse<GameSummaryDto>>.Failure("Page size must be between 1 and 100", ErrorType.Validation);

            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                return Result<PagedResponse<GameSummaryDto>>.Failure("Minimum price cannot exceed maximum price", ErrorType.Validation);

            // Get data from repository
            var (games, totalCount) = await _unitOfWork.Games.SearchGamesAsync(searchTerm, genreId, platformId, minPrice, maxPrice, pageNumber, pageSize, cancellationToken);

            // Map to DTOs
            var gameDtos = games.Select(g => g.ToSummaryDto()).ToList();

            // Create paged response
            var response = PagedResponse<GameSummaryDto>.Create(gameDtos, pageNumber, pageSize, totalCount);

            return Result<PagedResponse<GameSummaryDto>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching games");
            return Result<PagedResponse<GameSummaryDto>>.Failure("Failed to search games");
        }
    }

    public async Task<Result<GameDto>> GetGameByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result<GameDto>.Failure("Invalid game ID", ErrorType.Validation);

        try
        {
            var game = await _unitOfWork.Games.GetGameWithDetailsAsync(id, cancellationToken);

            if (game is null)
                return Result<GameDto>.Failure("Game not found", ErrorType.NotFound);

            return Result<GameDto>.Success(game.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting game {GameId}", id);
            return Result<GameDto>.Failure("Failed to retrieve game");
        }
    }

    public async Task<Result<IReadOnlyList<GameSummaryDto>>> GetActiveGamesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var games = await _unitOfWork.Games.GetActiveGamesAsync(cancellationToken);

            var gameDtos = games.Select(g => g.ToSummaryDto()).ToList();

            return Result<IReadOnlyList<GameSummaryDto>>.Success(gameDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active games");
            return Result<IReadOnlyList<GameSummaryDto>>.Failure("Failed to retrieve active games");
        }
    }

    // Command Operations

    public async Task<Result<GameDto>> CreateGameAsync(CreateGameRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating game: {Title}", request.Title);

        try
        {
            var validation = await ValidateGameCreationAsync(request, cancellationToken);
            if (validation.IsFailure)
                return Result<GameDto>.Failure(validation.Error,  ErrorType.Validation);

            var game = request.ToEntity();

            await _unitOfWork.Games.AddAsync(game, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var created = await _unitOfWork.Games.GetGameWithDetailsAsync(game.Id, cancellationToken);

            _logger.LogInformation("Game created: {GameId}", game.Id);

            return Result<GameDto>.Success(created!.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating game {Title}", request.Title);
            return Result<GameDto>.Failure("Failed to create game");
        }
    }

    public async Task<Result<GameDto>> UpdateGameAsync(int id, UpdateGameRequest request, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result<GameDto>.Failure("Invalid game ID", ErrorType.Validation);

        try
        {
            var game = await _unitOfWork.Games.GetByIdAsync(id, cancellationToken);
            if (game is null)
                return Result<GameDto>.Failure("Game not found", ErrorType.NotFound);

            var validation = await ValidateGameUpdateAsync(id, request, cancellationToken);
            if (validation.IsFailure)
                return Result<GameDto>.Failure(validation.Error, ErrorType.Validation);

            game.UpdateFromDto(request);

            await _unitOfWork.Games.UpdateAsync(game, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updated = await _unitOfWork.Games.GetGameWithDetailsAsync(id, cancellationToken);

            _logger.LogInformation("Game updated: {GameId}", id);

            return Result<GameDto>.Success(updated!.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating game {GameId}", id);
            return Result<GameDto>.Failure("Failed to update game");
        }
    }

    public async Task<Result> DeleteGameAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return Result.Failure("Invalid game ID", ErrorType.Validation);

        try
        {
            var game = await _unitOfWork.Games.GetByIdAsync(id, cancellationToken);
            if (game is null)
                return Result.Failure("Game not found", ErrorType.NotFound);

            var hasOrders = await _unitOfWork.OrderItems.ExistsAsync(oi => oi.GameId == id, cancellationToken);
            if (hasOrders)
                return Result.Failure("Cannot delete game with existing orders", ErrorType.Conflict);

            // Soft delete
            game.IsActive = false;
            game.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Games.UpdateAsync(game, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Game deleted: {GameId}", id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting game {GameId}", id);
            return Result.Failure("Failed to delete game");
        }
    }

    // Private validation methods

    private async Task<Result> ValidateGameCreationAsync(CreateGameRequest request, CancellationToken cancellationToken)
    {
        var titleExists = await _unitOfWork.Games.ExistsAsync(g => g.Title == request.Title, cancellationToken);
        if (titleExists)
            return Result.Failure("Game with this title already exists", ErrorType.Conflict);

        var genreExists = await _unitOfWork.Genres.ExistsAsync(g => g.Id == request.GenreId, cancellationToken);
        if (!genreExists)
            return Result.Failure("Invalid genre", ErrorType.NotFound);

        var platformExists = await _unitOfWork.Platforms.ExistsAsync(p => p.Id == request.PlatformId, cancellationToken);
        if (!platformExists)
            return Result.Failure("Invalid platform", ErrorType.NotFound);

        return Result.Success();
    }

    private async Task<Result> ValidateGameUpdateAsync(int gameId, UpdateGameRequest request, CancellationToken cancellationToken)
    {
        var titleExists = await _unitOfWork.Games.ExistsAsync(g => g.Title == request.Title && g.Id != gameId, cancellationToken);
        if (titleExists)
            return Result.Failure("Game with this title already exists", ErrorType.Conflict);

        var genreExists = await _unitOfWork.Genres.ExistsAsync(g => g.Id == request.GenreId, cancellationToken);
        if (!genreExists)
            return Result.Failure("Invalid genre", ErrorType.NotFound);

        var platformExists = await _unitOfWork.Platforms.ExistsAsync(p => p.Id == request.PlatformId, cancellationToken);
        if (!platformExists)
            return Result.Failure("Invalid platform", ErrorType.NotFound);

        return Result.Success();
    }
}
