using GameStore.DTOs.Requests;
using GameStore.DTOs.Responses;
using GameStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet(Name = nameof(GetAllUsers))]
    [ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all users.");

        var result = await _userService.GetAllUsersAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}", Name = nameof(GetUserById))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving user with ID: {UserId}", id);

        var result = await _userService.GetUserByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
