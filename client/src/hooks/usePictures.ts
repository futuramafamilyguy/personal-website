import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

import notFoundBoy from "../assets/404boy.png";
import notFoundGirl from "../assets/404girl.png";
import { useAuth } from "../contexts/AuthContext";
import { useViewFavorite } from "../contexts/ViewFavoriteContext";
import { useYear } from "../contexts/YearContext";
import {
  debouncedFetchMovies,
  makeDebouncedRequest,
} from "../api/debouncedFetch";
import Movie from "../types/Movie";

interface MovieResponse {
  movies: Movie[];
  totalCount: number;
}

const useMovies = (initialPage: number) => {
  const calculateItemsPerPage = () => {
    // using hard-coded values here since flex box dimensions are inconsistent
    // remember to change the width and height offset if any other elements change lol
    const containerWidth = window.innerWidth - 57;
    const itemWidth = 235; // media card width + horizontal padding
    const columns = Math.floor(containerWidth / itemWidth);
    const containerHeight = window.innerHeight - 280;
    const itemHeight = 150; // media card height + vertical padding
    const rows = Math.floor(containerHeight / itemHeight);

    return columns * rows - (isLoggedIn ? 1 : 0);
  };

  const isLoggedIn = useAuth();

  const [trigger, setTrigger] = useState(false);

  const [movies, setMovies] = useState<Movie[]>([]);
  const [currentPage, setCurrentPage] = useState(initialPage);
  const [totalPages, setTotalPages] = useState(0);
  const [itemsPerPage, setItemsPerPage] = useState(() =>
    calculateItemsPerPage()
  );
  const [loading, setLoading] = useState(false);
  const year = useYear();
  const viewFavorite = useViewFavorite();

  // fill any empty spaces in the flex content area with empty media cards so the positioning isnt scuffed AF
  const emptyMediaCardArray = Array.from(
    { length: itemsPerPage - movies.length },
    (_, index) => index + 1
  );

  useEffect(() => {
    const updateItemsPerPage = () => setItemsPerPage(calculateItemsPerPage());
    updateItemsPerPage();
    window.addEventListener("resize", updateItemsPerPage);

    return () => window.removeEventListener("resize", updateItemsPerPage);
  }, []);

  useEffect(() => {
    setCurrentPage(1);
  }, [year, totalPages]);

  useEffect(() => {
    const fetchMovies = () => {
      setLoading(true);
      if (viewFavorite) {
        makeDebouncedRequest(debouncedFetchMovies, {
          url: `/movies/${year}/favorites`,
        })
          .then((response: AxiosResponse<Movie[]>) => {
            const data = response.data;
            const updatedMovies = (<Movie[]>data).map((movie) => ({
              ...movie,
              imageUrl: setDefaultImageUrl(movie.imageUrl),
            }));
            setMovies(updatedMovies);
            setTotalPages(1);
          })
          .catch((error: any) => {
            setMovies([]);
            console.error("Error fetching movies:", error);
          })
          .finally(() => setLoading(false));
      } else {
        makeDebouncedRequest(debouncedFetchMovies, {
          url: `/movies/${year}?pageNumber=${currentPage}&pageSize=${itemsPerPage}`,
        })
          .then((response: AxiosResponse<MovieResponse>) => {
            const data = response.data;
            const updatedMovies = (<Movie[]>data.movies).map((movie) => ({
              ...movie,
              imageUrl: setDefaultImageUrl(movie.imageUrl),
            }));
            setMovies(updatedMovies);
            setTotalPages(Math.ceil(data.totalCount / itemsPerPage));
          })
          .catch((error: any) => {
            setMovies([]);
            console.error("Error fetching movies:", error);
          })
          .finally(() => setLoading(false));
      }
    };

    fetchMovies();
  }, [year, currentPage, itemsPerPage, viewFavorite, trigger]);

  const setDefaultImageUrl = (imageUrl: string) => {
    return imageUrl || (Math.random() < 0.5 ? notFoundBoy : notFoundGirl);
  };

  return {
    movies,
    currentPage,
    totalPages,
    itemsPerPage,
    loading,
    emptyMediaCardArray,
    setCurrentPage,
    setTrigger,
  };
};

export default useMovies;
