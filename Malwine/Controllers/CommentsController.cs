using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using Malwine.Contexts;
using Malwine.Models;
using Malwine.Services;

namespace Malwine.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class CommentsController(
  ApplicationDbContext _applicationDbContext,
  DtoFactory _dtoFactory,
  UserManager<User> _userManager
)
: ControllerBase
{
  readonly DbSet<Movie> _movies = _applicationDbContext.Movies;
  readonly DbSet<Comment> _comments = _applicationDbContext.Comments;

  [HttpGet]
  public async Task<IActionResult> Get(
    int movieId,
    [Range(0, int.MaxValue)] int offset = 0,
    [Range(0, 50)] int limit = 50
  )
  {
    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    var comments = movie.Comments.Skip(offset)
                                 .Take(limit);

    var commentsDtos = comments.Select(_dtoFactory.CreateComment);

    return Ok(commentsDtos);
  }


  [HttpPost]
  public async Task<IActionResult> Post(
    [FromForm] int movieId,
    [FromForm] string text
  )
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    var comment = new Comment()
    {
      Id = 0,
      Author = user,
      Text = text,
      Edited = false
    };

    movie.Comments.Add(comment);

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Edit(
    [FromForm] int movieId,
    [FromForm] int commentId,
    [FromForm] string text
  )
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    var comment = movie.Comments.FirstOrDefault(comment => comment.Id == commentId);
    if (comment == null) return NotFound();

    if (comment.Author != user) return Forbid();

    comment.Text = text;
    comment.Edited = true;

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Delete(
    [FromForm] int movieId,
    [FromForm] int commentId
  )
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Forbid();

    var movie = await _movies.FirstOrDefaultAsync(movie => movie.Id == movieId);
    if (movie == null) return NotFound();

    var comment = movie.Comments.FirstOrDefault(comment => comment.Id == commentId);
    if (comment == null) return NotFound();

    if (comment.Author != user) return Forbid();
   

    _comments.Remove(comment);

    await _applicationDbContext.SaveChangesAsync();

    return Ok();
  }
}