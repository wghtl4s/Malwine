namespace Malwine.Models.Dto;

public class Movie : LightMovie
{
  public required string Title { get; init; }
  public required string Description { get; init; }
  public required string[] Tags { get; init; }
  // public required LightUser Author { get; init; }
}