namespace Malwine.Models.Dto;

public class Comment : LightComment
{
  public required LightUser Author { get; init; }
  public required string Text { get; init; }
}