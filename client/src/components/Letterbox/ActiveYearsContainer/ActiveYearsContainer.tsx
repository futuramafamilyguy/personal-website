import { AxiosResponse } from "axios";
import React, { useEffect, useState } from "react";

import {
  debouncedFetchActiveYears,
  makeDebouncedRequest,
} from "../../../api/debouncedFetch";
import { useYear, useYearUpdate } from "../../../contexts/YearContext";
import CapsuleButton from "../../Common/CapsuleButton/CapsuleButton";
import styles from "./ActiveYearsContainer.module.css";

interface ActiveYearsResponse {
  activeYears: number[];
}

const ActiveYearsContainer: React.FC = () => {
  const year = useYear();
  const updateYear = useYearUpdate();
  const [activeYears, setActiveYears] = useState<number[]>([]);

  useEffect(() => {
    const fetchActiveYears = () => {
      makeDebouncedRequest(debouncedFetchActiveYears, {
        url: `/movies/active-years`,
      })
        .then((response: AxiosResponse<ActiveYearsResponse>) => {
          const startingYear = Math.min(...response.data.activeYears);
          const activeYears = [];
          const currentDate = new Date();
          const currentYear = currentDate.getFullYear();
          for (let i = currentYear; i >= startingYear; i--) {
            activeYears.push(i);
          }

          setActiveYears(activeYears);
        })
        .catch((error: any) => {
          console.error("Error fetching active years:", error);
        });
    };

    fetchActiveYears();
  }, []);

  const [startIndex, setStartIndex] = useState(0);
  const maxVisible = 4;

  const canGoLeft = startIndex > 0;
  const canGoRight = startIndex + maxVisible < activeYears.length;

  const handleLeft = () => {
    if (canGoLeft) setStartIndex(startIndex - 1);
  };

  const handleRight = () => {
    if (canGoRight) setStartIndex(startIndex + 1);
  };

  const visibleYears = activeYears.slice(startIndex, startIndex + maxVisible);

  if (activeYears.length === 0) {
    return null;
  }

  return (
    <div className={styles.buttonArea}>
      {activeYears.length > maxVisible && (
        <button
          onClick={handleLeft}
          disabled={!canGoLeft}
          className={styles.arrow}
        >
          {"<"}
        </button>
      )}
      {visibleYears.map((y) => (
        <CapsuleButton
          key={y}
          text={y.toString()}
          onClick={() => updateYear(y)}
          disabled={false}
          selected={year === y}
          width={"78px"}
        />
      ))}
      {activeYears.length > maxVisible && (
        <button
          onClick={handleRight}
          disabled={!canGoRight}
          className={styles.arrow}
        >
          {">"}
        </button>
      )}
    </div>
  );
};

export default ActiveYearsContainer;
