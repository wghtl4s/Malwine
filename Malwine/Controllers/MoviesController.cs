using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Malwine.Contexts;
using Malwine.Extensions;
using Malwine.Models;
using Malwine.Selectors;
using Malwine.Services;

namespace Malwine.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class MoviesController(
  ApplicationDbContext _applicationDbContext,
  DtoFactory _dtoFactory,
  UserManager<User> _userManager,
  ImageConverter _imageConverter
)
: ControllerBase
{
  readonly DbSet<Movie> _movies = _applicationDbContext.Movies;

  [HttpGet, OverloadSelector]
  public async Task<IActionResult> Get(
    int movieId
  )
  {
    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    var movieDto = _dtoFactory.CreateMovie(movie);

    return Ok(movieDto);
  }

  [HttpGet, OverloadSelector]
  public async Task<IActionResult> Get(
    [Range(0, int.MaxValue)] int offset,
    [Range(1, 50)] int limit
  )
  {
    var movies = await _movies.Skip(offset)
                             .Take(limit)
                             .ToListAsync();

    var moviesDtos = movies.Select(_dtoFactory.CreateMovie);

    return Ok(moviesDtos);
  }
  [HttpPost]
  public async Task<IActionResult> Create(
    [FromForm] string title,
    [FromForm] IFormFile cover,
    // [FromForm] IFormFile data,
    [FromForm] string description,
    [FromForm] string[] tags
  )
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = new Movie()
    {
      Id = 0,
      // Author = user,
      Comments = [],
      Cover = await _imageConverter.ConvertAsync(cover.OpenReadStream()),
      Description = description,
      Tags = tags,
      Title = title,
      // Data = data.OpenReadStream()
      //            .AsEnumerable()
      //            .ToArray()
      Likers = []
    };

    await _movies.AddAsync(movie);

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }
  [HttpPost]
  public async Task<IActionResult> Edit(
    int movieId,
    string? title,
    IFormFile? cover,
    string? description,
    string[]? tags
  )
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    if (title != null)
    {
      movie.Title = title;
    }

    if (cover != null)
    {
      movie.Cover = await _imageConverter.ConvertAsync(cover.OpenReadStream());
    }

    if (description != null)
    {
      movie.Description = description;
    }

    if (tags != null)
    {
      movie.Tags = tags;
    }

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Delete(int movieId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    _movies.Remove(movie);

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }

  // [HttpGet]
  // public async Task<IActionResult> Player(int movieId)
  // {
  //   var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
  //   if (movie == null) return NotFound();

  //   return File(movie.Data, "video/mp4");
  // }

  [HttpGet]
  public async Task<IActionResult> Cover(int movieId)
  {
    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    return File(movie.Cover, "image/webp");
  }

  [HttpPost]
  public async Task<IActionResult> Like(int movieId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    user.Liked.Add(movie);

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Unlike(int movieId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = user.Liked.FirstOrDefault(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    user.Liked.Remove(movie);

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }

  [HttpGet]
  public async Task<IActionResult> Recommendations()
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var likedTags = user.Liked.SelectMany(movie => movie.Tags)
                              .Distinct();

    var movies = await _movies
        .Where(movie => movie.Tags.Intersect(likedTags).Count() > 0)
        .ToListAsync();

    var moviesDtos = movies.Select(_dtoFactory.CreateMovie).ToList();

    return Ok(moviesDtos);
  }
}