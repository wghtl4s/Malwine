const BASE_URL = 'http://localhost:5162/api'; 

const apiMovies = {
    getPopularMovies: async (page = 1) => {
      const url = `${BASE_URL}/movies/get?offset=0&limit=50`; 
      const options = {
        method: 'GET',
       
      };
  
      try {
        const res = await fetch(url, options);
        if (!res.ok) {
          throw new Error(`HTTP error! status: ${res.status}`);
        }
        const json = await res.json();
        return json; 
      } catch (error) {
        console.error('Error fetching popular movies:', error);
        throw error;
      }
    },
  };
  
  export default apiMovies;