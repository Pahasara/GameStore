using GameStore.Common;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result.IsSuccess ? Ok(result.Value) : MapError(result.ErrorType, result.Error);
    }

    protected IActionResult HandleResult(Result result)
    {
        return result.IsSuccess ? NoContent() : MapError(result.ErrorType, result.Error);
    }

    protected IActionResult HandleCreated<T>(Result<T> result, string actionName, object routeValues)
    {
        return result.IsFailure ? HandleResult(result) : CreatedAtAction(actionName, routeValues, result.Value);
    }

    protected IActionResult ValidationProblem(Dictionary<string, string[]> errors)
    {
        return ValidationProblem(new ValidationProblemDetails(errors)
        {
            Title = "One or more validation errors occured",
            Status = StatusCodes.Status400BadRequest
        });
    }

    private IActionResult MapError(ErrorType? errorType, string errorMessage)
    {
        return errorType switch
        {
            ErrorType.NotFound => NotFound(new ProblemDetails
                {
                    Title = "Resource Not Found",
                    Detail = errorMessage,
                    Status = StatusCodes.Status404NotFound
                }),
            ErrorType.Conflict => Conflict(new ProblemDetails
                {
                    Title = "Conflict",
                    Detail = errorMessage,
                    Status = StatusCodes.Status409Conflict
                }),
            ErrorType.Forbidden =>
                StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
                {
                    Title = "Forbidden",
                    Detail = errorMessage,
                    Status = StatusCodes.Status403Forbidden
                }),
            ErrorType.Unauthorized =>
                Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = errorMessage,
                    Status = StatusCodes.Status401Unauthorized
                }),
            ErrorType.InternalServerError =>
                StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = errorMessage,
                    Status = StatusCodes.Status500InternalServerError
                }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = errorMessage,
                Status = StatusCodes.Status400BadRequest
            })
        };
    }
}
