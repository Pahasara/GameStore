using GameStore.Common;
using GameStore.Models.DTOs.Responses;

namespace GameStore.Services;

public interface IUserService
{
    Task<Result<UserDto>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<UserDto>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
}
