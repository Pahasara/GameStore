namespace GameStore.DTOs.Responses;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Computed properties
    public int GameCount { get; set; }
}
