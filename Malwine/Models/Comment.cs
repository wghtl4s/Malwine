namespace Malwine.Models;

public class Comment
{
  public required int Id { get; init; }
  public required virtual User Author { get; init; }
  public required string Text { get; set; }
  public required bool Edited { get; set; }
}