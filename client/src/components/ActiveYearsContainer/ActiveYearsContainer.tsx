import React, { useEffect, useState } from "react";

import { useYear, useYearUpdate } from "../../contexts/YearContext";
import CapsuleButton from "../CapsuleButton/CapsuleButton";
import styles from "./ActiveYearsContainer.module.css";

const ActiveYearsContainer: React.FC = () => {
  const year = useYear();
  const updateYear = useYearUpdate();
  const [activeYears, setActiveYears] = useState<number[]>([]);

  useEffect(() => {
    fetch("https://localhost:7044/pictures/active-years")
      .then((response) => response.json())
      .then((data) => {
        const startingYear = Math.min(...data.activeYears);
        const activeYears = [];
        const currentDate = new Date();
        const currentYear = currentDate.getFullYear();
        for (let i = currentYear; i >= startingYear; i--) {
          activeYears.push(i);
        }

        setActiveYears(activeYears);
      })
      .catch((error) => console.error("Error fetching years:", error));
  }, []);

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
