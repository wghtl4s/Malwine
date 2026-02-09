import React from 'react';
import '../scss/button.scss';
const LikedButton = ({ movieId }) => {
  const handleLike = async () => {

    console.log(movieId); 

    if (!movieId) {
      console.error('No movieId provided');
      return;
    }

    try {
      const response = await fetch(
        `http://localhost:5162/api/movies/like?movieId=${movieId}`,
        {
          method: 'POST',
          credentials: 'include',
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        console.error('Error liking the movie:', errorData.message);
      }
    } catch (error) {
      console.error('Network error:', error);
    }
  };

  return (
    <button onClick={handleLike} className='button--like'>
      Like
    </button>
  );
};

export default LikedButton;
