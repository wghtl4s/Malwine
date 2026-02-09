import React, { useState } from 'react';
import '../scss/loginform.scss';

const LoginForm = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append('userName', userName);
    formData.append('password', password);

    try {
      const response = await fetch('http://localhost:5162/api/users/login', {
        method: 'POST',
        body: formData,
        credentials: 'include',
      });

      if (response.ok) {
       
        sessionStorage.setItem('userName', userName);
        localStorage.setItem('userName', userName);
        setSuccess('Успішний логін!');
        setError('');
      } else if (response.status === 400) {
        setError('Неправильне ім’я або пароль!');
        setSuccess('');
      } else {
        setError('Помилка. Спробуйте ще раз.');
        setSuccess('');
      }
    } catch (err) {
      setError('Помилка у мережі.');
      setSuccess('');
    }
  };

  return (
    <div className="login-form-container">
      <h2>Вхід</h2>
      <form className="login-form" onSubmit={handleSubmit}>
        <div>
          <label htmlFor="userName">username:</label>
          <input
            type="text"
            id="userName"
            className="form-input"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="password">password:</label>
          <input
            type="password"
            id="password"
            className="form-input"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="submit-button">
          Увійти
        </button>
      </form>
      {error && <p className="error-message">{error}</p>}
      {success && <p className="success-message">{success}</p>}
      <div className="back-to-home">Назад на головну</div>

    </div>
  );
};

export default LoginForm;
