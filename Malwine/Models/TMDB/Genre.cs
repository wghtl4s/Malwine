using System.Text.Json.Serialization;

namespace Malwine.Models.TMDB;

class Genre
{
  [JsonPropertyName("id")]
  public required int Id { get; init; }

  [JsonPropertyName("name")]
  public required string Name { get; init; }
}