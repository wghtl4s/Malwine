// const BASE_URL = 'https://api.themoviedb.org/3'; 
// const API_KEY =
//   'eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3MWEwZmZjZTJkNTVlZjEwODJlMzc0YTVkM2ZiNzUyOSIsIm5iZiI6MTcyODA2MTc5NS42NjAyMTQsInN1YiI6IjY2ZjNiYWUxNzA5MWQ1NzU1ZDY5ZTQ2NCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.Loki2TelBv2jvuIygW5KiNShQIHGsmOvfyHDeC4TOiE';

// const tmdbService = {
//   getPopularMovies: async (page = 1) => {
//     const url = `${BASE_URL}/movie/popular?language=en-US&page=${page}`; 
//     const options = {
//       method: 'GET',
//       headers: {
//         accept: 'application/json',
//         Authorization: `Bearer ${API_KEY}`,
//       },
//     };

//     try {
//       const res = await fetch(url, options);
//       if (!res.ok) {
//         throw new Error(`HTTP error! status: ${res.status}`);
//       }
//       const json = await res.json();
//       return json.results; 
//     } catch (error) {
//       console.error('Error fetching popular movies from TMDb:', error);
//       throw error;
//     }
//   },
// };

// export default tmdbService;
