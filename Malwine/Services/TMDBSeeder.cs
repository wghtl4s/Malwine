using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Malwine.Contexts;
using Malwine.Models;

using TMDB = Malwine.Models.TMDB;

namespace Malwine.Services;

public class TMDBSeeder(
  ApplicationDbContext _applicationDbContext,
  HttpClient _http,
  ImageConverter _imageConverter
)
{
  readonly DbSet<Movie> _movies = _applicationDbContext.Movies;

  public async Task SeedIfNeeded()
  {
    if (await _movies.AnyAsync()) return;

    var genreMovieListResponse = await _http.GetAsync("https://api.themoviedb.org/3/genre/movie/list?language=uk");
    var genreMovieList = (await genreMovieListResponse.Content.ReadFromJsonAsync<TMDB.GenreMovieListResponse>())!;

    var tags = genreMovieList.Genres.ToDictionary(genre => genre.Id, genre => genre.Name);

    var discoverMoviesResponse = await _http.GetAsync("https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&language=uk&page=1&sort_by=popularity.desc");
    var discoverMovies = (await discoverMoviesResponse.Content.ReadFromJsonAsync<TMDB.DiscoverMovieResponse>())!;

    var movies = await Task.WhenAll(discoverMovies.Results.Select(async (movie) => {
      var posterResponse = await _http.GetAsync("https://image.tmdb.org/t/p/w500" + movie.PosterPath);
      var poster = await posterResponse.Content.ReadAsStreamAsync();

      return new Movie() {
        Title = movie.Title,
        Comments = [],
        Cover = await _imageConverter.ConvertAsync(poster),
        Description = movie.Overview,
        Id = 0,
        Tags = movie.GenreIds.Select(id => tags[id])
                             .ToArray(),
        Likers = []
      };
    }));

    await _movies.AddRangeAsync(movies);

    await _applicationDbContext.SaveChangesAsync();
  }
}