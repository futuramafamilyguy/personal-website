import { AxiosResponse } from "axios";
import React, { useEffect, useState } from "react";

import {
  debouncedFetchActiveYears,
  makeDebouncedRequest,
} from "../../../api/debouncedFetch";
import { useYear } from "../../../contexts/YearContext";
import styles from "./MobileActiveYearsContainer.module.css";
import { useNavigate } from "react-router-dom";

interface ActiveYearsResponse {
  activeYears: number[];
}

const MobileActiveYearsContainer: React.FC = () => {
  const year = useYear();
  const navigate = useNavigate();
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

  if (activeYears.length === 0) {
    return null;
  }

  return (
    <div className={styles.buttonArea}>
      {activeYears.map((y) => (
        <span
          key={y}
          className={`${styles.yearText} ${year === y ? styles.selected : ""}`}
          onClick={() => navigate(`/letterbox/${y}`)}
        >
          {y}
        </span>
      ))}
    </div>
  );
};

export default MobileActiveYearsContainer;
