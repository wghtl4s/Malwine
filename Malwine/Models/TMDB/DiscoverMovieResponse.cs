using System.Text.Json.Serialization;

namespace Malwine.Models.TMDB;

class DiscoverMovieResponse
{
  [JsonPropertyName("results")]
  public required Movie[] Results { get; init; }
}