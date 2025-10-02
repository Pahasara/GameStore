using GameStore.Models.Entities;

namespace GameStore.Repositories;

public interface IUnitOfWork : IDisposable
{
    // Specialized repositories
    IGameRepository Games { get; }
    IUserRepository Users { get; }
    IOrderRepository Orders { get; }

    // Generic repositories
    IRepository<Review> Reviews { get; }
    IRepository<Genre> Genres { get; }
    IRepository<Platform> Platforms { get; }
    IRepository<OrderItem> OrderItems { get; }

    // Transaction management
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
