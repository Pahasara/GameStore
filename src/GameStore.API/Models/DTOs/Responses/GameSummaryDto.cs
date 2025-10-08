namespace GameStore.Models.DTOs.Responses;

public class GameSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }

    // Nested properties
    public int GenreId { get; set; }
    public string GenreName { get; set; } = string.Empty;
    public int PlatformId { get; set; }
    public string PlatformName { get; set; } = string.Empty;

    // Computed properties
    public string FormattedPrice => Price.ToString("C");
    public string ReleaseDateFormatted => ReleaseDate.ToString("MMM yyyy");
}