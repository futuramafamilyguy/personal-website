import { AxiosResponse } from "axios";
import React, { useEffect, useState } from "react";

import { useYear, useYearUpdate } from "../../../contexts/YearContext";
import {
  debouncedFetchActiveYears,
  makeDebouncedRequest,
} from "../../../personalWebsiteApi";
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
        url: `/pictures/active-years`,
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
        <CapsuleButton
          key={y}
          text={y.toString()}
          onClick={() => updateYear(y)}
          disabled={false}
          selected={year === y}
        />
      ))}
    </div>
  );
};

export default ActiveYearsContainer;
