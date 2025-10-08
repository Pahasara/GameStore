using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Requests;

public class SearchGameRequest
{
    [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
    public string? SearchTerm { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Genre ID must be positive")]
    public int? GenreId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Platform ID must be positive")]
    public int? PlatformId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Minimum price cannot be negative")]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Minimum price cannot be negative")]
    public decimal? MaxPrice { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Minimum price cannot be negative")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;

    public GameSortOrder SortBy { get; set; } = GameSortOrder.Title;

    public bool IncludeInactive { get; set; } = false;

    public bool IsValid(out string error)
    {
        if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
        {
            error = "Minimum price cannot be greater than maximum price";
            return false;
        }

        error = string.Empty;
        return true;
    }
}
