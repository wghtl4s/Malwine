using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Malwine.Contexts;
using Malwine.Models;
using Malwine.Services;
using System.Linq;

namespace Malwine.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class UsersController(
  // ApplicationDbContext _applicationDbContext,
  UserManager<User> _userManager,
  SignInManager<User> _signInManager,
  DtoFactory _dtoFactory,
  ImageConverter _imageConverter
)
: ControllerBase
{
 [HttpGet]
public async Task<IActionResult> Get(string userName)
{
    var user = await _userManager.Users
        .Include(user => user.Liked) 
        .FirstOrDefaultAsync(user => user.UserName == userName);
        
    if (user == null) return NotFound();

    var userDto = new
    {
        user.UserName,
        user.About,
        user.Avatar,
        LikedMovies = user.Liked.Select(movie => new
        {
            movie.Id,
            movie.Title,
            movie.Cover,
            movie.Description,
            movie.Tags
        }).ToList()
    };

    return Ok(userDto);
}


  [HttpPost]
  public async Task<IActionResult> Register(
    [FromForm] string userName,
    [FromForm] string password,
    [FromForm] IFormFile avatar,
    [FromForm] string about
  )
  {
    var user = new User() {
      UserName = userName,
      Avatar = await _imageConverter.ConvertAsync(avatar.OpenReadStream()),
      About = about,
      Liked = [],
      Comments = []
    };

    await _userManager.CreateAsync(user, password);

    await _signInManager.SignInAsync(user, true);

    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Login(
    [FromForm] string userName,
    [FromForm] string password
  )
  {
    await _signInManager.PasswordSignInAsync(userName, password, true, false);

    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();

    return Ok();
  }

  [HttpGet]
  public async Task<IActionResult> Avatar(string userName)
  {
    var user = await _userManager.FindByNameAsync(userName);
    if (user == null) return NotFound();

    return File(user.Avatar, "image/webp");
  }

  [HttpGet]
  public async Task<IActionResult> Comments(string userName)
  {
    var user = await _userManager.FindByNameAsync(userName);
    if (user == null) return NotFound();

    var comments = user.Comments;

    var commentsDtos = comments.Select(_dtoFactory.CreateComment);

    return Ok(commentsDtos);
  }
}