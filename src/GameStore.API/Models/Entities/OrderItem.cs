namespace GameStore.Models.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int OrderId { get; set; }
    public int GameId { get; set; }

    // Navigation properties
    public Order Order { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
