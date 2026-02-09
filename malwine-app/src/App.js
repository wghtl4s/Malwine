import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './scss/loginform.scss';
import './scss/app.scss';
import './scss/button.scss';
import './scss/registerform.scss';

import {
  Categories,
  Header,
  SortMovies,
  MovieList,
  LoginForm,
  RegisterForm,
  Button,
  UserProfile,
  Recommendations,
  MovieDetails,
} from './components';
import apiMovies from './services/apimovies';
import LogoutButton from './components/LogoutButton';

function App() {
  const [movies, setMovies] = useState([]);
  const [error, setError] = useState(null);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [currentUserName, setCurrentUserName] = useState('');

  useEffect(() => {
    const storedUserName = sessionStorage.getItem('userName'); 
    if (storedUserName) {
      setIsLoggedIn(true);
      setCurrentUserName(storedUserName);
    }
  }, []);

  useEffect(() => {
    const getMovies = async () => {
      try {
        const data = await apiMovies.getPopularMovies();
        setMovies(data);
      } catch (error) {
        console.error('Помилка:', error);
        setError(`Помилка: ${error.message}`);
      }
    };

    getMovies();
  }, []);

  return (
    <Router>
      <div className="App">
        <Header />
        <div className="auth-buttons">
          <Button onClick={() => (window.location.href = '/login')} className="button">
            Вхід
          </Button>
          <span className="divider">|</span>
          <Button onClick={() => (window.location.href = '/register')} className="button">
            Регістрація
          </Button>
          {isLoggedIn && (
            <>
              <span className="divider">|</span>
              <Button onClick={() => (window.location.href = '/account')} className="button">
                Профіль
              </Button>
              <span className="divider">|</span>
              <Button onClick={() => (window.location.href = '/recommendations')} className="button">
                Рекомендації 
              </Button> 
              <span className="divider">|</span>
              <LogoutButton setIsLoggedIn={setIsLoggedIn} setCurrentUserName={setCurrentUserName} />
            </>
          )}
        </div>
        <main>
          <Routes>
            <Route
              path="/login"
              element={
                <LoginForm setIsLoggedIn={setIsLoggedIn} setCurrentUserName={setCurrentUserName} />
              }
            />
            <Route path="/register" element={<RegisterForm />} />
            <Route path="/account" element={<UserProfile currentUserName={currentUserName} />} />
            <Route path="/recommendations" element={<Recommendations />} /> 
            <Route path="/movies/:movieId" element={<MovieDetails />} />
            <Route
              path="/"
              element={
                <div className="main-content">
                  <h2>Знайди для себе кінострічки </h2>
                  <Categories
                    onClickItem={(name) => console.log(name)}
                    items={['Фантастика', 'Мультфільми', 'Драма', 'Жахи']}
                  />
                  <SortMovies items={['popularity', 'rating', 'alphabet']} />
                  {error && <p className="error-message">{error}</p>}
                  <MovieList movies={movies}/>
                </div>
              }
            />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
