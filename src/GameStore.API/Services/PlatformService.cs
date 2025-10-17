using GameStore.Common;
using GameStore.Extensions;
using GameStore.Models.DTOs.Responses;
using GameStore.Models.Entities;
using GameStore.Repositories;

namespace GameStore.Services;

public class PlatformService : BaseService<Platform, PlatformDto>, IPlatformService
{
    public PlatformService(IUnitOfWork unitOfWork, ILogger<PlatformService> logger) : base(unitOfWork, logger)
    {
    }

    protected override IRepository<Platform> Repository => UnitOfWork.Platforms;
    protected override PlatformDto MapToDto(Platform entity) => entity.ToDto();

    public async Task<Result<PlatformDto>> GetPlatformByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<IReadOnlyList<PlatformDto>>> GetAllPlatformsAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllAsync(cancellationToken);
    }
}
