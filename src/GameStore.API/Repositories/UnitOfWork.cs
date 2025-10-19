using GameStore.Data;
using GameStore.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameStore.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameStoreDbContext _context;
    private IDbContextTransaction? _transaction;

    private readonly Lazy<IGameRepository> _games;
    private readonly Lazy<IUserRepository> _users;
    private readonly Lazy<IOrderRepository> _orders;
    private readonly Lazy<IRepository<Review>> _reviews;
    private readonly Lazy<IRepository<Genre>> _genres;
    private readonly Lazy<IRepository<Platform>> _platforms;
    private readonly Lazy<IRepository<OrderItem>> _orderItems;

    public UnitOfWork(GameStoreDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _games = new Lazy<IGameRepository>(() => new GameRepository(_context));
        _users = new Lazy<IUserRepository>(() => new UserRepository(_context));
        _orders = new Lazy<IOrderRepository>(() => new OrderRepository(_context));
        _reviews = new Lazy<IRepository<Review>>(() => new Repository<Review>(_context));
        _genres = new Lazy<IRepository<Genre>>(() => new Repository<Genre>(_context));
        _platforms = new Lazy<IRepository<Platform>>(() => new Repository<Platform>(_context));
        _orderItems = new Lazy<IRepository<OrderItem>>(() => new Repository<OrderItem>(_context));
    }

    public IGameRepository Games => _games.Value;
    public IUserRepository Users => _users.Value;
    public IOrderRepository Orders => _orders.Value;
    public IRepository<Review> Reviews => _reviews.Value;
    public IRepository<Genre> Genres => _genres.Value;
    public IRepository<Platform> Platforms => _platforms.Value;
    public IRepository<OrderItem> OrderItems => _orderItems.Value;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            if (_transaction is not null)
            {
                await RollbackTransactionAsync(cancellationToken);
            }
            throw;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("Transaction already started.");

        // EF Core transaction wraps database transaction
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            // Make all changes permanent
            await _transaction.CommitAsync(cancellationToken);

            // Clean up transaction resource
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            // undo all changes since BeginTransaction
            await _transaction.RollbackAsync(cancellationToken);

            // Clean up transaction resource
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            // clean up transaction if still active
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
