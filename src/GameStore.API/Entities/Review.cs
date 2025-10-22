namespace GameStore.Entities;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int UserId { get; set; }
    public int GameId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
