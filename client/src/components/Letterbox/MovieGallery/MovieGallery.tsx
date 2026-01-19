import { useEffect, useRef, useState } from "react";
import React from "react";
import { useLocation, useNavigate } from "react-router-dom";

import heart from "../../../assets//svg/heart.png";
import flower from "../../../assets/motifs/flower.svg";
import { useAuth } from "../../../contexts/AuthContext";
import { useYear } from "../../../contexts/YearContext";
import { useIsMobile } from "../../../hooks/useIsMobile";
import useMoviesV2 from "../../../hooks/useMoviesV2";
import Movie from "../../../types/Movie";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import CreateCinemaModal from "../CreateCinemaModal/CreateCinemaModal";
import CreateMovieModal from "../CreateMovieModal/CreateMovieModal";
import MovieMonthRow from "../MonthRow/MonthRow";
import MovieModal from "../MovieModal/MovieModal";
import NewMediaCard from "../NewMediaCard/NewMediaCard";
import NomineeRow from "../NomineeRow/NomineeRow";
import MovieStatsRow from "../StatsRow/StatsRow";
import styles from "./MovieGallery.module.css";

const MovieGallery: React.FC = () => {
  const { movies, loading, setTrigger } = useMoviesV2();

  const [selectedMovie, setSelectedMovie] = useState<Movie | null>(null);

  const isLoggedIn = useAuth();
  const year = useYear();
  const location = useLocation();
  const modalOpen =
    location.pathname.includes("/focus") && selectedMovie !== null;
  const [movieIndex, setMovieIndex] = useState<number | null>(null);
  const [createMovieModalOpen, setCreateMovieModalOpen] = useState(false);
  const [createCinemaModalOpen, setCreateCinemaModalOpen] = useState(false);
  const [nominees, setNominees] = useState<Movie[]>();
  const [viewNominees, setViewNominees] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    setNominees(movies.filter((m: Movie) => m.isNominated));
  }, [movies]);

  const openModal = (movie: Movie, nominated = false) => {
    setSelectedMovie(movie);

    if (nominated) {
      setViewNominees(true);
      setMovieIndex(nominees!.indexOf(movie));
    } else {
      setViewNominees(false);
      setMovieIndex(movies.indexOf(movie));
    }

    navigate(`/letterbox/${year}/focus`);
  };

  const closeModal = () => {
    setViewNominees(false);
    setSelectedMovie(null);
    setMovieIndex(null);
    navigate(`/letterbox/${year}`);
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
    setCreateMovieModalOpen(true);
  };

  const openNewCinemaModal = () => {
    setCreateCinemaModalOpen(true);
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

  const isMobile = useIsMobile();

  const icons: Record<string, string> = {
    flower,
    heart,
  };

  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollIntoView({
        behavior: "auto",
        block: "center",
      });
    }
  }, [selectedMovie]);

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
    } else if (isMobile && modalOpen) {
      return (
        <div className={styles.mobileMovieList}>
          <div className={styles.mobileMovieHeader}>
            <span className={styles.headerTitle}>
              {year}{" "}
              {viewNominees ? (
                <img className={styles.favouriteIconMed} src={icons["heart"]} />
              ) : (
                "watchlist"
              )}
            </span>

            <span className={styles.backButton} onClick={closeModal}>
              back
            </span>
          </div>

          {(viewNominees ? nominees! : movies).map((movie, index, list) => {
            const showMonthHeader =
              !viewNominees &&
              (index === 0 || movie.month !== list[index - 1].month);

            return (
              <React.Fragment key={movie.id}>
                {showMonthHeader && (
                  <h4 className={styles.monthHeader}>
                    {months[12 - movie.month]}
                  </h4>
                )}

                <div
                  className={styles.mobileMovieDetails}
                  ref={movie.id === selectedMovie?.id ? scrollRef : null}
                >
                  <div className={styles.imageContainer}>
                    <img
                      src={
                        viewNominees && movie?.altImageUrl
                          ? movie?.altImageUrl
                          : movie?.imageUrl
                      }
                      alt={movie.name}
                      className={styles.modalImage}
                      loading="lazy"
                    />
                  </div>

                  <div className={styles.textBlock}>
                    <div className={styles.titleRow}>
                      <h5
                        className={styles.movieTitle}
                        style={{
                          color: movie?.isKino ? "#E3BF46" : "white",
                        }}
                      >
                        {movie.name} ({movie.releaseYear})
                        {movie.isNominated && (
                          <img
                            className={styles.favouriteIconSmall}
                            src={icons[movie.motif] || icons["heart"]}
                          />
                        )}
                      </h5>
                    </div>

                    <div className={styles.cinemaInfo}>
                      {movie.cinema.name}, {movie.cinema.city}
                    </div>
                  </div>
                </div>
              </React.Fragment>
            );
          })}
        </div>
      );
    } else {
      return (
        <div className={styles.movieGallery}>
          {isLoggedIn ? (
            <>
              <div className={styles.newEntitiesRow}>
                <h5>after this one, we're watching four more</h5>
                <hr />
                <NewMediaCard
                  imageSrc={
                    "https://cdn.allenmaygibson.com/images/static/the-fourth.webp"
                  }
                  onClick={() => openNewMovieModal(null)}
                />
                <NewMediaCard
                  imageSrc={
                    "https://cdn.allenmaygibson.com/images/static/archives.jpg"
                  }
                  onClick={() => openNewCinemaModal()}
                />
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
              isOpen={createMovieModalOpen}
              onClose={() => {
                setCreateMovieModalOpen(false);
                setSelectedMovie(null);
              }}
              movie={selectedMovie}
              setTrigger={() =>
                setTrigger((prevTrigger: boolean) => !prevTrigger)
              }
            />
          ) : null}
          {isLoggedIn ? (
            <CreateCinemaModal
              isOpen={createCinemaModalOpen}
              onClose={() => {
                setCreateCinemaModalOpen(false);
              }}
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
