using GameStore.DTOs.Requests;
using GameStore.DTOs.Responses;
using GameStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : BaseApiController
{
    private readonly IGameService _gameService;
    private readonly ILogger<GamesController> _logger;

    public GamesController(IGameService gameService, ILogger<GamesController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpGet(Name = nameof(SearchGames))]
    [ProducesResponseType(typeof(PagedResponse<GameSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchGames(
        [FromQuery] string? searchTerm,
        [FromQuery] int? genreId,
        [FromQuery] int? platformId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Searching games: term={SearchTerm}, genre={GenreId}, platform={PlatformId}, page={PageNumber}",
            searchTerm, genreId, platformId, pageNumber);
        
        var result = await _gameService.SearchGamesAsync(searchTerm, genreId, platformId, minPrice, maxPrice,
            pageNumber, pageSize, cancellationToken);
        
        return HandleResult(result);
    }

    [HttpGet("active", Name = nameof(GetActiveGames))]
    [ProducesResponseType(typeof(IReadOnlyList<GameSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveGames(CancellationToken cancellationToken = default)
    {
        var result = await _gameService.GetActiveGamesAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetGameById))]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _gameService.GetGameByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost(Name = nameof(CreateGame))]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateGame(
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        
        _logger.LogInformation("Creating new game: {Title}", request.Title);
        var result = await _gameService.CreateGameAsync(request, cancellationToken);
        
        if(result.IsFailure)
            return HandleResult(result);
        
        _logger.LogInformation("Game created successfully: {GameId}", result.Value!.Id);
        
        return HandleCreated(result, nameof(GetGameById), new { id = result.Value.Id });
    }

    [HttpPut("{id:int}", Name = nameof(UpdateGame))]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGame( 
        int id,
        [FromBody] UpdateGameRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        
        _logger.LogInformation("Updating game: {GameId}", id);
        var result = await _gameService.UpdateGameAsync(id, request, cancellationToken);
        
        if(result.IsSuccess)
            _logger.LogInformation("Game updated successfully: {GameId}", id);
        
        return HandleResult(result);
    }

    [HttpDelete("{id:int}", Name = nameof(DeleteGame))]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGame(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting game: {GameId}", id);
        var result = await _gameService.DeleteGameAsync(id, cancellationToken);
        
        if(result.IsSuccess)
            _logger.LogInformation("Game deleted successfully: {GameId}", id);
        
        return HandleResult(result);
    }
}