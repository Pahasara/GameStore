using GameStore.Common;
using GameStore.Extensions;
using GameStore.Models.DTOs.Responses;
using GameStore.Models.Entities;
using GameStore.Repositories;

namespace GameStore.Services;

public class GenreService : BaseService<Genre, GenreDto>, IGenreService
{
    public GenreService(IUnitOfWork unitOfWork, ILogger<GenreService> logger) : base(unitOfWork, logger) { }

    protected override IRepository<Genre> Repository => UnitOfWork.Genres;
    protected override GenreDto MapToDto(Genre entity) => entity.ToDto();

    public async Task<Result<GenreDto>> GetGenreByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<IReadOnlyList<GenreDto>>> GetAllGenresAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllAsync(cancellationToken);
    }
}
