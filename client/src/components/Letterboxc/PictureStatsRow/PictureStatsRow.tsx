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
  const [topCinemas, setTopCinemas] = useState<
    {
      name: string;
      visits: number;
    }[]
  >([]);

  useEffect(() => {
    setTotalPictureCount(pictures.length);
    setNewReleaseCount(
      pictures.filter(
        (p) => p.yearReleased === year || p.yearReleased === year - 1
      ).length
    );
    setRereleaseCount(
      pictures.filter(
        (p) => p.yearReleased !== year && p.yearReleased !== year - 1
      ).length
    );
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
          <h5>{year} Stats</h5>
        </div>
        <hr />
        <div className={styles.statsContainer}>
          <div className={styles.stat}>
            <span className={styles.statKey}>Total Pictures</span>:{" "}
            {totalPictureCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>New Releases</span>:{" "}
            {newReleaseCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>Re-releases</span>:{" "}
            {rereleaseCount}
          </div>
          <div className={styles.stat}>
            <span className={styles.statKey}>Total Spending</span>: No
          </div>
          <div className={styles.lastStat}>
            <div className={styles.statKey}>Most Frequent Cinemas</div>
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
