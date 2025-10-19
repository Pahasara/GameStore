using GameStore.Common;
using GameStore.DTOs.Responses;

namespace GameStore.Services;

public interface IGenreService
{
    Task<Result<IReadOnlyList<GenreDto>>> GetAllGenresAsync(CancellationToken cancellationToken = default);

    Task<Result<GenreDto>> GetGenreByIdAsync(int id, CancellationToken cancellationToken = default);
}
