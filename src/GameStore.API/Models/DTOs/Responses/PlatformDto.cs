namespace GameStore.Models.DTOs.Responses;

public class PlatformDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Create { get; set; }
    public int GameCount { get; set; }
}
