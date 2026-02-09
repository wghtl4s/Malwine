using System.Collections.Generic;

namespace Malwine.Models;

public class Movie
{
  public required int Id { get; init; }
  public required string Title { get; set; }
  public required byte[] Cover { get; set; }
  public required string Description { get; set; }
  public required string[] Tags { get; set; }
  // public required byte[] Data { get; init; }
  public required virtual ICollection<Comment> Comments { get; init; }
    public required virtual ICollection<User> Likers { get; init; }
}