import { useEffect, useState } from "react";

import Picture from "../../../types/Picture";
import styles from "./PictureStatsRow.module.css";

interface PictureStatsRowProps {
  pictures: Picture[];
  year: number;
}

const PictureStatsRow: React.FC<PictureStatsRowProps> = ({
  pictures,
  year,
}) => {
  const [totalPictureCount, setTotalPictureCount] = useState<number>();
  const [newReleaseCount, setNewReleaseCount] = useState<number>();
  const [rereleaseCount, setRereleaseCount] = useState<number>();
  const [cinemaCount, setCinemaCount] = useState<number>();
  const [cityCount, setCityCount] = useState<number>();
  const [topCinemas, setTopCinemas] = useState<
    {
      name: string;
      visits: number;
    }[]
  >([]);

  useEffect(() => {
    setTotalPictureCount(pictures.length);
    setNewReleaseCount(pictures.filter((p) => p.isNewRelease).length);
    setRereleaseCount(pictures.filter((p) => !p.isNewRelease).length);
    setCinemaCount(new Set(pictures.map((p) => p.cinema.id)).size);
    setCityCount(new Set(pictures.map((p) => p.cinema.city)).size);
    setTopCinemas(calculateTopCinemas());
  }, [pictures]);

  const calculateTopCinemas = () => {
    const cinemaMap: { [cinemaName: string]: number } = {};

    pictures.forEach((picture) => {
      if (cinemaMap[picture.cinema.name]) {
        cinemaMap[picture.cinema.name]++;
      } else {
        cinemaMap[picture.cinema.name] = 1;
      }
    });

    const sortedCinemas = Object.entries(cinemaMap)
      .map(([cinema, count]) => ({ name: cinema, visits: count }))
      .sort((a, b) => b.visits - a.visits);

    return sortedCinemas.slice(0, 3);
  };

  const renderContent = () => {
    return (
      <div className={styles.pictureStatsRow}>
        <div className={styles.title}>
          <h5>{year} stats</h5>
        </div>
        <hr />
        <div className={styles.statsContainer}>
          <div className={styles.stat}>
            <span className={styles.statKey}>total pictures</span>:{" "}
            {totalPictureCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>new releases</span>:{" "}
            {newReleaseCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>re-releases</span>:{" "}
            {rereleaseCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>total spending</span>: no
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>unique cinemas</span>:{" "}
            {cinemaCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>unique cities</span>: {cityCount}
          </div>
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

export default PictureStatsRow;
