using GameStore.Models.Entities;

namespace GameStore.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    // order management opertaions
    Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderWithItemsAsync(int orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
}