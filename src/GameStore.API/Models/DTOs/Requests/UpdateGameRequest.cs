using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.DTOs.Requests;

public class UpdateGameRequest
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999.99, ErrorMessage = "Price must be between $0.01 and $999.99")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Release date is required.")]
    public DateTime ReleaseDate { get; set; }

    [StringLength(300, ErrorMessage = "Image URL cannot exceed 300 characters.")]
    [Url(ErrorMessage = "Image URL must be a valid URL.")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Genre is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "GenreId must be a positive integer.")]
    public int GenreId { get; set; }

    [Required(ErrorMessage = "Platform is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "PlatformId must be a positive integer.")]
    public int PlatformId { get; set; }

    public bool IsActive { get; set; } = true;
}
