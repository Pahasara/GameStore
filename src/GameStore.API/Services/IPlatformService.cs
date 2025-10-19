using GameStore.Common;
using GameStore.DTOs.Responses;

namespace GameStore.Services;

public interface IPlatformService
{
    Task<Result<IReadOnlyList<PlatformDto>>> GetAllPlatformsAsync(CancellationToken cancellationToken = default);
    Task<Result<PlatformDto>> GetPlatformByIdAsync(int id, CancellationToken cancellationToken = default);
}
