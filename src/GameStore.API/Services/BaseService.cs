using GameStore.Common;
using GameStore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public abstract class BaseService<TEntity, TDto>
    where TEntity : class
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ILogger Logger;

    protected BaseService(IUnitOfWork unitOfWork, ILogger logger)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected abstract IRepository<TEntity> Repository { get; }
    protected abstract TDto MapToDto(TEntity entity);
    protected virtual string EntityName => typeof(TEntity).Name;

    public virtual async Task<Result<TDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            Logger.LogWarning("Invalid {EntityName} ID: {Id}", EntityName, id);
            return Result<TDto>.Failure($"Invalid {typeof(TEntity).Name} ID");
        }

        try
        {
            var entity = await Repository.GetByIdAsync(id, cancellationToken);

            if (entity is null)
            {
                Logger.LogWarning("Error getting {EntityName} with ID: {Id}", EntityName, id);
                return Result<TDto>.Failure($"{EntityName} not found");
            }

            return Result<TDto>.Success(MapToDto(entity));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting {EntityType} with ID {id}", EntityName, id);
            return Result<TDto>.Failure($"Failed to retrieve {EntityName}");
        }
    }

    public virtual async Task<Result<IReadOnlyList<TDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entities = await Repository.GetAllAsync(cancellationToken);
            var dtos = entities.Select(MapToDto).ToList();

            Logger.LogInformation("Retrieved {Count} {EntityName} records", entities.Count(), EntityName);
            return Result<IReadOnlyList<TDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting all {EntityType}", EntityName);
            return Result<IReadOnlyList<TDto>>.Failure($"Failed to retrieve {EntityName} list");
        }
    }

    protected async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await Repository.ExistsAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);
    }

    protected async Task<Result> ValidateEntityExistsAsync<T>(
        int id,
        string entityName,
        Func<int, CancellationToken, Task<bool>> existsCheck,
        CancellationToken cancellationToken)
    {
        var exists = await existsCheck(id, cancellationToken);
        return exists ? Result.Success() : Result.Failure($"Invalid {entityName}");
    }
}
