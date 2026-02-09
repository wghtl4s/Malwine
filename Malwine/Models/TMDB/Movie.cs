using System.Text.Json.Serialization;

namespace Malwine.Models.TMDB;

class Movie
{
  [JsonPropertyName("genre_ids")]
  public required int[] GenreIds { get; init; }

  [JsonPropertyName("title")]
  public required string Title { get; init; }

  [JsonPropertyName("overview")]
  public required string Overview { get; init; }

  [JsonPropertyName("poster_path")]
  public required string PosterPath { get; init; }
}