using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace Malwine.Models;

public class User : IdentityUser
{
  public required byte[] Avatar { get; init; }
  public required string About { get; init; }
  public required virtual ICollection<Movie> Liked { get; init; }
  public required virtual ICollection<Comment> Comments { get; init; }
}