import React from 'react';
import '../scss/button.scss';
const LogoutButton = ({ setIsLoggedIn, setCurrentUserName }) => {
  const handleLogout = async () => {
    try {
      const response = await fetch('http://localhost:5162/api/users/logout', {
        method: 'POST',
        credentials: 'include',
      });

      if (response.ok) {
        sessionStorage.removeItem('userName');
        localStorage.removeItem('userName');
        setIsLoggedIn(false);
        setCurrentUserName('');
        alert('Ви вийшли з системи.');
      } else {
        alert('Помилка при виході.');
      }
    } catch (err) {
      alert('Помилка у мережі при виході.');
    }
  };

  return (
    <button onClick={handleLogout} className="button">
      Вийти
    </button>
  );
};

export default LogoutButton;
