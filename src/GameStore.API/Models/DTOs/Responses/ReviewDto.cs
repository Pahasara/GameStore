namespace GameStore.Models.DTOs.Responses;

public class ReviewDto
{
    public int Id { get; set; }
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // User info
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;

    // Computed properties
    public string TimeAgo => CalculateTimeAgo(CreatedAt);
    public bool IsEdited => UpdatedAt > CreatedAt.AddMinutes(1);

    private static string CalculateTimeAgo(DateTime createdAt)
    {
        var timeSpan = DateTime.UtcNow - createdAt;

        return timeSpan switch
        {
            { TotalDays: >= 365 } => $"{(int)(timeSpan.TotalDays / 365)} year(s) ago.",
            { TotalDays: >= 30 } => $"{(int)(timeSpan.TotalDays / 30)} month(s) ago.",
            { TotalDays: >= 1 } => $"{(int)(timeSpan.TotalDays)} day(s) ago.",
            { TotalHours: >= 1 } => $"{(int)(timeSpan.TotalHours)} hour(s) ago.",
            { TotalMinutes: >= 1 } => $"{(int)(timeSpan.TotalMinutes)} minute(s) ago.",
            _ => "Just Now"
        };
    }
}
