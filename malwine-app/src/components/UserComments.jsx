import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../scss/comments.scss';

const UserComments = ({ userName }) => {
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchComments = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5162/api/users/comments?userName=${userName}`,
        );
        setComments(response.data);
      } catch (err) {
        setError('Не вдалося завантажити коментарі.');
      } finally {
        setLoading(false);
      }
    };

    fetchComments();
  }, [userName]);

  if (loading) return <div>Завантаження...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="usercomments-section">
      <h2>Коментарі користувача {userName}</h2>
      {comments.length > 0 ? (
        <ul className="usercomments-list">
          {comments.map((comment) => (
            <li key={comment.id} className="usercomments-item">
              <div className="usercomments-author">
                <p>{comment.text}</p>
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <p>Коментарів поки немає.</p>
      )}
    </div>
  );
};

export default UserComments;
