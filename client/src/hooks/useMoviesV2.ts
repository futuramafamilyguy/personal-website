import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

import { useYear } from "../contexts/YearContext";
import {
  debouncedFetchMovies,
  makeDebouncedRequest,
} from "../api/debouncedFetch";
import Movie from "../types/Movie";

interface MovieResponse {
  movies: Movie[];
}

const useMoviesV2 = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(false);
  const year = useYear();

  // used to reload movies when a new movie is added (see CreateMovieModal)
  const [trigger, setTrigger] = useState(false);

  useEffect(() => {
    const fetchMovies = () => {
      setLoading(true);

      makeDebouncedRequest(debouncedFetchMovies, {
        url: `/movies/${year}`,
      })
        .then((response: AxiosResponse<MovieResponse>) => {
          const data = response.data;
          const updatedMovies = (<Movie[]>data.movies).map((movie) => ({
            ...movie,
            imageUrl: setDefaultImageUrl(movie.imageUrl),
          }));
          setMovies(updatedMovies);
        })
        .catch((error: any) => {
          setMovies([]);
          console.error("error fetching movies:", error);
        })
        .finally(() => setLoading(false));
    };

    fetchMovies();
  }, [year, trigger]);

  const setDefaultImageUrl = (imageUrl: string) => {
    return (
      imageUrl ||
      (Math.random() < 0.5
        ? "https://cdn.allenmaygibson.com/images/static/sun.jpg"
        : "https://cdn.allenmaygibson.com/images/static/light.jpg")
    );
  };

  return {
    movies,
    loading,
    setTrigger,
  };
};

export default useMoviesV2;
