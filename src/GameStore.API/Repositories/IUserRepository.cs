using GameStore.Models.Entities;

namespace GameStore.Repositories;

public interface IUserRepository : IRepository<User>
{
    // Identity operations
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithOrdersAsync(int userId, CancellationToken cancellationToken = default);
}
