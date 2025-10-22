using GameStore.DTOs.Responses;
using GameStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GenresController : BaseApiController
{
    private readonly IGenreService _genreService;
    private readonly ILogger<GenresController> _logger;

    public GenresController(IGenreService genreService, ILogger<GenresController> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    [HttpGet(Name = nameof(GetAllGenres))]
    [ProducesResponseType(typeof(IReadOnlyList<GenreDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGenres(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all genres");

        var result = await _genreService.GetAllGenresAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetGenreByIdAsync))]
    [ProducesResponseType(typeof(GenreDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGenreByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving genre with ID: {GenreId}", id);
        
        var result = await _genreService.GetGenreByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
