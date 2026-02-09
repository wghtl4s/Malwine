import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import '../scss/moviedet.scss';
import Comments from './Comments';
import LikedButton from './LikedButton';
import UnlikedButton from './UnlikedButton';


const MovieDetails = () => {
  const { movieId } = useParams();
  const [movie, setMovie] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        const response = await fetch(`http://localhost:5162/api/movies/get?movieId=${movieId}`, {
          method: 'GET',
          credentials: 'include',
        });

        if (response.ok) {
          const data = await response.json();
          setMovie(data);
        } else {
          const errorData = await response.json();
          setError(errorData.message || 'Ошибка при получении информации о фильме');
        }
      } catch (err) {
        setError('Сетевая ошибка');
      }
    };

    fetchMovieDetails();
  }, [movieId]);

  if (error) {
    return <div style={{ color: 'red' }}>{error}</div>;
  }

  if (!movie) {
    return <div>Загрузка данных о фильме...</div>;
  }

  return (
    <div className="movie-details">
      <h2>{movie.title}</h2>
      <img
        src={`http://localhost:5162/api/movies/cover?movieId=${movie.movieId}`}
        alt={movie.title}
        style={{ width: '300px' }}
      />
      <p>{movie.description}</p>
      <p>Теги: {movie.tags.join(', ')}</p>
      <LikedButton />
      <UnlikedButton />
      <div className="comments-section">
        <Comments movieId={movieId} />
      </div>
    </div>
  );
};

export default MovieDetails;
