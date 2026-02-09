import '../scss/recommendations.scss';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'; // Импортируем Link


const Recommendations = () => {
  const [recommendedMovies, setRecommendedMovies] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchRecommendations = async () => {
      try {
        const response = await fetch('http://localhost:5162/api/movies/recommendations', {
          method: 'GET',
          credentials: 'include', 
        });

        if (response.ok) {
          const data = await response.json();
          setRecommendedMovies(data);
        } else {
          const errorData = await response.json();
          setError(errorData.message || 'Ошибка при получении рекомендаций');
        }
      } catch (err) {
        setError('Сетевая ошибка');
      }
    };

    fetchRecommendations();
  }, []);

  if (error) {
    return <div style={{ color: 'red' }}>{error}</div>;
  }

  if (recommendedMovies.length === 0) {
    return <div className="recommendations">Отакої! Ви не додали жодного фільму до списку улюблених.</div>;
  }

  return (
    <div className="recommendations">
      <h2 className="recommendations__title">Рекомендації для вас</h2>
      <ul className="recommendations__list">
        {recommendedMovies.map((movie) => (
          <li className="recommendations__item" key={movie.movieId}>
            <Link to={`/movies/${movie.movieId}`} className="recommendations__link">
              <h3 className="recommendations__item-title">{movie.title}</h3>
              <div style={{ display: 'flex' }}>
                <img
                  className="recommendations__item-cover"
                  src={`http://localhost:5162/api/movies/cover?movieId=${movie.movieId}`}
                  alt={movie.title}
                />
                <p className="recommendations__item-description">{movie.description}</p>
              </div>
            </Link>
            <p className="recommendations__item-tags">Теги: {movie.tags.join(', ')}</p>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Recommendations;
