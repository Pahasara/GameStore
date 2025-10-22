namespace GameStore.DTOs;

public enum GameSortOrder
{
    Title,
    Price,
    PriceLowToHigh = Price,
    PriceHighToLow,
    ReleaseDate,
    ReleaseDateNewest = ReleaseDate,
    ReleaseDateOldest,
    CreatedAt,
    CreatedAtNewest = CreatedAt,
    CreatedAtOldest
}
