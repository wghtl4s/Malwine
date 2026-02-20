import React, { useState } from 'react';
import axios from 'axios';
import '../scss/registerform.scss';
const RegisterForm = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [about, setAbout] = useState('');
  const [avatar, setAvatar] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append('userName', userName);
    formData.append('password', password);
    formData.append('about', about);
    if (avatar) {
      formData.append('avatar', avatar);
    }

    try {
      const response = await axios.post('http://localhost:5162/api/users/register', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      if (response.data.success) {
        setSuccess('Реєстрація пройшла успішно! Ви можете увійти в систему.');
        setError('');
      }
    } catch (err) {
      setError(err.response.data.message || 'помилка при реєстрації.');
      setSuccess('');
    }
  };

  return (
    <div className="register-form-container">
      <h2>Реєстрація</h2>
      <form className="register-form" onSubmit={handleSubmit}>
        <div>
          <label htmlFor="userName">Ім’я користувача:</label>
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
          <label htmlFor="password">Пароль:</label>
          <input
            type="password"
            id="password"
            className="form-input"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="about">Інформація про себе:</label>
          <textarea
            id="about"
            className="form-textarea"
            value={about}
            onChange={(e) => setAbout(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label htmlFor="avatar">Аватар:</label>
          <div className="custom-file-upload">
            <input
              type="file"
              id="avatar"
              className="form-file-input"
              accept="image/*"
              onChange={(e) => setAvatar(e.target.files[0])}
              hidden
            />
            <label htmlFor="avatar" className="custom-upload-button">
              Оберіть файл
            </label>
            {avatar && <span className="file-name">{avatar.name}</span>}{' '}
           
          </div>
        </div>

        <button type="submit" className="submit-button">
          Зареєструватися
        </button>
      </form>
      {error && <p className="error-message">{error}</p>}
      {success && <p className="success-message">{success}</p>}
    </div>
  );
};

export default RegisterForm;
