import { useEffect, useState } from "react";

import Movie from "../../../types/Movie";
import styles from "./StatsRow.module.css";
import { useIsMobile } from "../../../hooks/useIsMobile";

interface StatsRowProps {
  movies: Movie[];
  year: number;
}

const StatsRow: React.FC<StatsRowProps> = ({ movies, year }) => {
  const [totalCount, setTotalCount] = useState<number>();
  const [newReleaseCount, setNewReleaseCount] = useState<number>();
  const [retroCount, setRetroCount] = useState<number>();
  const [cinemaCount, setCinemaCount] = useState<number>();
  const [cityCount, setCityCount] = useState<number>();
  const [topCinemas, setTopCinemas] = useState<
    {
      name: string;
      visits: number;
    }[]
  >([]);

  useEffect(() => {
    setTotalCount(movies.length);
    setNewReleaseCount(movies.filter((m) => !m.isRetro).length);
    setRetroCount(movies.filter((m) => m.isRetro).length);
    setCinemaCount(new Set(movies.map((m) => m.cinema.id)).size);
    setCityCount(new Set(movies.map((m) => m.cinema.city)).size);
    setTopCinemas(calculateTopCinemas());
  }, [movies]);

  const calculateTopCinemas = () => {
    const cinemaMap: { [cinemaName: string]: number } = {};

    movies.forEach((movie) => {
      if (cinemaMap[movie.cinema.name]) {
        cinemaMap[movie.cinema.name]++;
      } else {
        cinemaMap[movie.cinema.name] = 1;
      }
    });

    const sortedCinemas = Object.entries(cinemaMap)
      .map(([cinema, count]) => ({ name: cinema, visits: count }))
      .sort((a, b) => b.visits - a.visits);

    return sortedCinemas.slice(0, 3);
  };

  const isMobile = useIsMobile();

  const renderContent = () => {
    return (
      <div className={styles.statsRow}>
        <div className={styles.title}>
          <h5>{year} stats</h5>
        </div>
        <hr />
        <div className={styles.statsContainer}>
          <div className={styles.stat}>
            <span className={styles.statKey}>total movies</span>: {totalCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>new releases</span>:{" "}
            {newReleaseCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>retro classics</span>: {retroCount}
          </div>
          {isMobile ? null : (
            <div className={styles.stat}>
              <span className={styles.statKey}>unique cinemas</span>:{" "}
              {cinemaCount}
            </div>
          )}
          {isMobile ? null : (
            <div className={styles.stat}>
              <span className={styles.statKey}>unique cities</span>: {cityCount}
            </div>
          )}
          <div className={styles.lastStat}>
            <div className={styles.statKey}>most frequent cinemas</div>
            {topCinemas.map((cinema, index) => (
              <div key={index} className={styles.cinema}>
                {cinema.name}: {cinema.visits}
              </div>
            ))}
          </div>
        </div>
      </div>
    );
  };

  return renderContent();
};

export default StatsRow;
