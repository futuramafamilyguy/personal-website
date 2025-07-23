import { useEffect, useState } from "react";

import { useAuth } from "../../../contexts/AuthContext";
import { useYear } from "../../../contexts/YearContext";
import useMoviesV2 from "../../../hooks/useMoviesV2";
import Movie from "../../../types/Movie";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import NomineeRow from "../NomineeRow/NomineeRow";
import NewMediaCard from "../NewMediaCard/NewMediaCard";
import CreateMovieModal from "../CreateMovieModal/CreateMovieModal";
import MovieModal from "../MovieModal/MovieModal";
import MovieMonthRow from "../MonthRow/MonthRow";
import MovieStatsRow from "../StatsRow/StatsRow";
import styles from "./MovieGallery.module.css";

const MovieGallery: React.FC = () => {
  const { movies, loading, setTrigger } = useMoviesV2();

  const [selectedMovie, setSelectedMovie] = useState<Movie | null>(null);

  const isLoggedIn = useAuth();
  const year = useYear();
  const [modalOpen, setModalOpen] = useState(false);
  const [movieIndex, setMovieIndex] = useState<number | null>(null);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [nominees, setNominees] = useState<Movie[]>();
  const [viewNominees, setViewNominees] = useState(false);

  useEffect(() => {
    setNominees(movies.filter((m: Movie) => m.isNominated));
  }, [movies]);

  const openModal = (movie: Movie, nominated = false) => {
    setSelectedMovie(movie);

    if (nominated) {
      setViewNominees(true);
      setMovieIndex(nominees!.indexOf(movie));
    } else {
      setMovieIndex(movies.indexOf(movie));
    }

    setModalOpen(true);
  };

  const closeModal = () => {
    setViewNominees(false);
    setSelectedMovie(null);
    setMovieIndex(null);
    setModalOpen(false);
  };

  const handlePrevPic = () => {
    if (movieIndex !== null) {
      var movieList = movies;
      if (viewNominees) {
        movieList = nominees!;
      }
      setSelectedMovie(movieList[movieIndex - 1]);
      setMovieIndex(movieIndex - 1);
    }
  };

  const handleNextPic = () => {
    if (movieIndex !== null) {
      var movieList = movies;
      if (viewNominees) {
        movieList = nominees!;
      }
      setSelectedMovie(movieList[movieIndex + 1]);
      setMovieIndex(movieIndex + 1);
    }
  };

  const openNewMovieModal = (movie: Movie | null) => {
    setSelectedMovie(movie);
    setCreateModalOpen(true);
  };

  const months: string[] = [
    "december",
    "november",
    "october",
    "september",
    "august",
    "july",
    "june",
    "may",
    "april",
    "march",
    "february",
    "january",
  ];

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
    } else {
      return (
        <div className={styles.movieGallery}>
          {isLoggedIn ? (
            <>
              <div className={styles.newMovieRow}>
                <h5>after this one, we're watching four more</h5>
                <hr />
                <NewMediaCard onClick={() => openNewMovieModal(null)} />
              </div>
            </>
          ) : null}
          {nominees && nominees.length > 0 ? (
            <NomineeRow
              movies={movies.filter((m: Movie) => m.isNominated === true)}
              year={year}
              movieOnClick={(m: Movie) => openModal(m, true)}
              movieEditable={isLoggedIn}
              movieOnClickEdit={(m: Movie) => openNewMovieModal(m)}
            />
          ) : null}
          {months.map((month, index) => {
            var currentDate = new Date();
            var currentMonth = 12;
            if (year === currentDate.getFullYear()) {
              currentMonth = currentDate.getMonth() + 1;
              if (12 - index > currentMonth) {
                return null;
              }
            }

            return (
              <MovieMonthRow
                key={index}
                movies={movies.filter((m: Movie) => m.month === 12 - index)}
                month={month}
                movieOnClick={(m: Movie) => openModal(m)}
                movieEditable={isLoggedIn}
                movieOnClickEdit={(m: Movie) => openNewMovieModal(m)}
              />
            );
          })}
          <hr />
          <MovieStatsRow movies={movies} year={year!} />
          <MovieModal
            isOpen={modalOpen}
            onClose={closeModal}
            movie={selectedMovie}
            handlePrev={handlePrevPic}
            handleNext={handleNextPic}
            prev={movieIndex !== 0}
            next={
              movieIndex !==
              (viewNominees ? nominees!.length : movies.length) - 1
            }
            isAltImage={viewNominees}
          />
          {isLoggedIn ? (
            <CreateMovieModal
              isOpen={createModalOpen}
              onClose={() => {
                setCreateModalOpen(false);
                setSelectedMovie(null);
              }}
              movie={selectedMovie}
              setTrigger={() =>
                setTrigger((prevTrigger: boolean) => !prevTrigger)
              }
            />
          ) : null}
        </div>
      );
    }
  };

  return renderContent();
};

export default MovieGallery;
