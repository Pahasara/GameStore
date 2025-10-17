using GameStore.Common;
using GameStore.Extensions;
using GameStore.Models.DTOs.Responses;
using GameStore.Models.Entities;
using GameStore.Repositories;

namespace GameStore.Services;

public class UserService : BaseService<User, UserDto>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger) : base(unitOfWork, logger) { }

    protected override IRepository<User> Repository => UnitOfWork.Users;
    protected override UserDto MapToDto(User entity) => entity.ToDto();

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllAsync(cancellationToken);
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken);
    }
}
