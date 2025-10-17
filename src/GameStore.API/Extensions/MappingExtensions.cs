using GameStore.Models.DTOs.Requests;
using GameStore.Models.DTOs.Responses;
using GameStore.Models.Entities;

namespace GameStore.Extensions;

public static class MappingExtensions
{
    public static GameDto ToDto(this Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate,
            ImageUrl = game.ImageUrl,
            IsActive = game.IsActive,
            CreatedAt = game.CreatedAt,
            UpdatedAt = game.UpdatedAt,

            // Map nestedd entities
            Genre = game.Genre is not null
                ? new GenreDto
                {
                    Id = game.Genre.Id,
                    Name = game.Genre.Name,
                    Description = game.Genre.Description,
                    CreatedAt = game.Genre.CreatedAt
                }
            : new GenreDto(),

            Platform = game.Platform is not null
                ? new PlatformDto
                {
                    Id = game.Platform.Id,
                    Name = game.Platform.Name,
                    CreatedAt = game.Platform.CreatedAt
                }
            : new PlatformDto(),

            Reviews = game.Reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                UserId = r.UserId,
                Username = r.User.Username

            }).ToList() ?? []
        };
    }

    public static GameSummaryDto ToSummaryDto(this Game game)
    {
        return new GameSummaryDto
        {
            Id = game.Id,
            Title = game.Title,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate,
            ImageUrl = game.ImageUrl,
            IsActive = game.IsActive,
            GenreId = game.GenreId,
            GenreName = game.Genre.Name,
            PlatformId = game.PlatformId,
            PlatformName = game.Platform.Name,
            AverageRating = game.Reviews.Any() ? game.Reviews.Average(r => r.Rating) : 0
        };
    }

    public static Game ToEntity(this CreateGameRequest request)
    {
        return new Game
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            ReleaseDate = request.ReleaseDate,
            ImageUrl = request.ImageUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            GenreId = request.GenreId,
            PlatformId = request.PlatformId,
        };
    }

    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description,
            CreatedAt = genre.CreatedAt,
            GameCount = genre.Games.Count
        };
    }

    public static PlatformDto ToDto(this Platform platform)
    {
        return new PlatformDto
        {
            Id = platform.Id,
            Name = platform.Name,
            Description = platform.Description,
            CreatedAt = platform.CreatedAt,
            GameCount = platform.Games.Count
        };
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
        };
    }

    public static void UpdateFromDto(this Game game, UpdateGameRequest request)
    {
        game.Title = request.Title;
        game.Description = request.Description;
        game.Price = request.Price;
        game.ReleaseDate = request.ReleaseDate;
        game.ImageUrl = request.ImageUrl;
        game.GenreId = request.GenreId;
        game.PlatformId = request.PlatformId;
        game.UpdatedAt = DateTime.UtcNow;
    }
}
