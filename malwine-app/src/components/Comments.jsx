import React, { useState, useEffect } from 'react';
import '../scss/comments.scss';

const Comments = ({ movieId }) => {
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [editCommentId, setEditCommentId] = useState(null);
  const [editCommentText, setEditCommentText] = useState('');
  const [error, setError] = useState('');
  const [commentError, setCommentError] = useState('');
  const userName = localStorage.getItem('userName');

  const fetchComments = async () => {
    try {
      const response = await fetch(`http://localhost:5162/api/comments/get?movieId=${movieId}`, {
        method: 'GET',
        credentials: 'include',
      });

      if (response.ok) {
        const data = await response.json();

        setComments(data);
      } else {
        setError('Помилка при отриманні комментаря');
      }
    } catch (err) {
      setError('Мережева помилка');
    }
  };

  useEffect(() => {
    fetchComments();
  }, [movieId]);
  const handleCommentSubmit = async (e) => {
    e.preventDefault();

    const userId = localStorage.getItem('userId'); 
    const formData = new FormData();
    formData.append('movieId', movieId);
    formData.append('userId', userId); 
    formData.append('text', newComment);

    try {
        const response = await fetch(`http://localhost:5162/api/comments/post`, {
            method: 'POST',
            credentials: 'include',
            body: formData,
        });

        if (response.ok) {
            await fetchComments();
            setNewComment('');
        } else {
            const errorData = await response.json();
            setCommentError(errorData.message || 'Помилка при відправці');
        }
    } catch (err) {
        setCommentError('Отакої! Мережева помилка');
    }
};


  const handleEditCommentSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append('movieId', movieId);
    formData.append('commentId', editCommentId);
    formData.append('text', editCommentText);

    try {
      const response = await fetch(`http://localhost:5162/api/comments/edit`, {
        method: 'POST',
        credentials: 'include',
        body: formData,
      });

      if (response.ok) {
        await fetchComments();
        setEditCommentId(null);
        setEditCommentText('');
      } else {
        const errorData = await response.json();
        setCommentError(errorData.message || 'Помилка при редагуванні');
      }
    } catch (err) {
      setCommentError('Отакої! Мережева помилка');
    }
  };
  const handleDeleteComment = async (commentId) => {
    console.log('Comment ID to delete:', commentId); 
    const formData = new FormData();
    formData.append('movieId', movieId);
    formData.append('commentId', commentId);

    console.log('Deleting comment:', {
      movieId: movieId,
      commentId: commentId,
    });

    try {
      const response = await fetch(`http://localhost:5162/api/comments/delete`, {
        method: 'POST',
        credentials: 'include',
        body: formData,
      });

      if (response.ok) {
        await fetchComments();
      } else {
        const errorData = await response.json();
        setCommentError(errorData.message || 'Помилка при видаленні');
      }
    } catch (err) {
      setCommentError('Отакої! Мережева помилка');
    }
  };

  return (
    <div className="comments-section">
      <h3>Коментарі</h3>
      {comments.length > 0 ? (
        comments.map((comment) => (
          <div key={comment.id} className="comment">
            <p>
              <strong>{comment.author.userName}</strong>: {comment.text}
            </p>
            {comment.edited && <span>(Edited)</span>}
            <button
              onClick={() => {
                setEditCommentId(comment.commentId);
                setEditCommentText(comment.text);
              }}>
              Редагувати
            </button>
            <button
              onClick={() => {
                console.log('Attempting to delete comment with ID:', comment.commentId);
                handleDeleteComment(comment.commentId);
              }}>
              Видалити
            </button>
          </div>
        ))
      ) : (
        <p>Залиште коментар першими!</p>
      )}

      <form onSubmit={editCommentId ? handleEditCommentSubmit : handleCommentSubmit}>
        <textarea
          value={editCommentId ? editCommentText : newComment}
          onChange={(e) =>
            editCommentId ? setEditCommentText(e.target.value) : setNewComment(e.target.value)
          }
          placeholder="Залиште свою думку."
        />
        <button type="submit">{editCommentId ? 'Оновити коментар' : 'надіслати'}</button>
      </form>

      {error && <p style={{ color: 'red' }}>{error}</p>}
      {commentError && <p style={{ color: 'red' }}>{commentError}</p>}
    </div>
  );
};

export default Comments;
