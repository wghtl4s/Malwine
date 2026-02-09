import React, { useEffect, useState } from 'react';
import apiMovies from '../services/apimovies';
import LikeButton from './LikedButton'
import UnlikedButton from './UnlikedButton';
import '../scss/button.scss';
const MovieList = () => {
  const [movies, setMovies] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    (async () => {
      try {
        const dataPage1 = await apiMovies.getPopularMovies(1);
        setMovies(dataPage1);
      } catch (error) {
        console.error('Error fetching movies:', error);
        setError(error.message);
      }
    })();
  }, []);

  return (
    <div className="movies-container">
      
      {error && <p className="error-message">{error}</p>}
      {movies.length === 0 && !error ? (
        <p>No movies found.</p>
      ) : (
        <div className="movie-list">
          {movies.map((movie) => (
            <div key={movie.movieId} className="movie">
              <img
                src={`http://localhost:5162/api/movies/cover?movieId=${movie.movieId}`}
                alt={movie.title}
                className="movie-poster"
              />
              <h3 className="movie-title">{movie.title}</h3>
              
              <LikeButton movieId={movie.movieId} />
              <UnlikedButton movieId={movie.movieId} />
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MovieList;
