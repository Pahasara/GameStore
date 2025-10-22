using GameStore.DTOs.Responses;
using GameStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : BaseApiController
{
    private readonly IGenreService _genreService;    
    private readonly ILogger<PlatformsController> _logger;

    public PlatformsController(IGenreService genreService, ILogger<PlatformsController> logger)
    {
        _genreService = genreService;
        _logger = logger;
    }

    [HttpGet(Name = nameof(GetAllPlatforms))]
    [ProducesResponseType(typeof(IReadOnlyList<PlatformDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPlatforms(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all platforms.");

        var result = await _genreService.GetAllGenresAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetPlatformById))]
    [ProducesResponseType(typeof(PlatformDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPlatformById(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving platform with ID: {PlatformId}", id);
        
        var result = await _genreService.GetGenreByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
