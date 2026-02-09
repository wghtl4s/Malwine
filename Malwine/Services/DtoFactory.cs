using System.Linq;
using Malwine.Models;

using Dto = Malwine.Models.Dto;

namespace Malwine.Services;

public class DtoFactory
{
  public Dto.User CreateUser(User user)
  {
    return new Dto.User() {
      UserName = user.UserName!,
      About = user.About,
      Liked = user.Liked.Select(CreateMovie)
    };
  }

  public Dto.Movie CreateMovie(Movie movie)
  {
    return new Dto.Movie() {
      MovieId = movie.Id,
      // Author = CreateLightUser(movie.Author),
      Title = movie.Title,
      Description = movie.Description,
      Tags = movie.Tags
    };
  }

  public Dto.LightUser CreateLightUser(User user)
  {
    return new Dto.LightUser() {
      UserName = user.UserName!
    };
  }

  public Dto.Comment CreateComment(Comment comment)
  {
    return new Dto.Comment() {
      CommentId = comment.Id,
      Author = CreateLightUser(comment.Author),
      Text = comment.Text
    };
  }

  public Dto.LightMovie CreateLightMovie(Movie movie)
  {
    return new Dto.LightMovie() {
      MovieId = movie.Id
    };
  }

  public Dto.LightComment CreateLightComment(Comment comment)
  {
    return new Dto.LightComment() {
      CommentId = comment.Id
    };
  }
}