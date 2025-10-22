namespace GameStore.DTOs.Responses;

public class GameDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Nested DTOs
    public GenreDto Genre { get; set; } = null!;
    public PlatformDto Platform { get; set; } = null!;

    // Collections
    public List<ReviewDto> Reviews { get; set; } = [];

    // Computed properties
    public decimal AverageRating => Reviews.Any()
        ? Reviews.Average(r => r.Rating)
        : 0m;
    public int ReviewCount => Reviews.Count;
    public string FormattedPrice => Price.ToString("C"); // $99.99 format    
    public string ReleaseDateFormatted => ReleaseDate.ToString("MMM dd, yyyy"); // Oct 07, 2025 format
}
