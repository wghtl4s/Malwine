using System.Text.Json.Serialization;

namespace Malwine.Models.TMDB;

class GenreMovieListResponse
{
  [JsonPropertyName("genres")]
  public required Genre[] Genres { get; init; }
}