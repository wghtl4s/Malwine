using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Malwine.Models;

namespace Malwine.Contexts;

public class ApplicationDbContext(
  DbContextOptions<ApplicationDbContext> options
)
: IdentityDbContext<User>(options)
{
  public required DbSet<Movie> Movies { get; init; }
  public required DbSet<Comment> Comments { get; init; }
}