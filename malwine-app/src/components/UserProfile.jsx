import React, { useEffect, useState } from 'react';
import '../scss/userprofile.scss';
import UserComments from './UserComments';

// адреса сервера в константу
const API_BASE_URL = 'http://localhost:5162/api/users';

const UserProfile = () => {
  const [user, setUser] = useState(null);
  const [error, setError] = useState('');
  const currentUserName = sessionStorage.getItem('userName');

  useEffect(() => {
    const fetchUserData = async () => {
      if (!currentUserName) {
        setError('username == null');
        return;
      }

      try {
        const response = await fetch(
          `${API_BASE_URL}/get?userName=${currentUserName}`, // Використовуємо константу для URL
          {
            method: 'GET',
            credentials: 'include',
          },
        );

        if (response.ok) {
          const data = await response.json();
          console.log(data);
          setUser(data);
        } else {
          const errorData = await response.json();
          setError(errorData.message || '!get username');
        }
      } catch (err) {
        setError('error network');
      }
    };

    fetchUserData();
  }, [currentUserName]);

  if (error) {
    return <div style={{ color: 'red' }}>{error}</div>;
  }

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <div className="user-profile">
      <h2 className="user-profile__title">Профіль</h2>
      {user.avatar && (
        <img
          src={`data:image/webp;base64,${user.avatar}`}
          alt="Avatar"
          className="user-profile__avatar"
        />
      )}
      <div className="user-profile__info">
        <p>І'мя користувача: {user.userName}</p>
        <p>Про себе: {user.about}</p>
      </div>

      {user.likedMovies && user.likedMovies.length > 0 ? (
        <div className="user-profile__liked-movies">
          <h3>Вподобайки</h3>
          <ul className="user-profile__movie-list">
            {user.likedMovies.map((movie) => (
              <li key={movie.movieId} className="user-profile__movie-item">
                <p>{movie.title}</p>
              </li>
            ))}
          </ul>
        </div>
      ) : (
        <p>Немає вподобайок</p>
      )}
      <UserComments userName={currentUserName}/>
    </div>
  );
};

export default UserProfile;