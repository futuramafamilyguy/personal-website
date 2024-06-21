import React, { useEffect, useState } from "react";

import { useYear, useYearUpdate } from "../../contexts/YearContext";
import CapsuleButton from "../CapsuleButton/CapsuleButton";
import styles from "./ActiveYearsContainer.module.css";

import api from "../../api";

const ActiveYearsContainer: React.FC = () => {
  const year = useYear();
  const updateYear = useYearUpdate();
  const [activeYears, setActiveYears] = useState<number[]>([]);

  useEffect(() => {
    const fetchActiveYears = async () => {
      try {
        const response = await api.get("/pictures/active-years");
        const startingYear = Math.min(...response.data.activeYears);
        const activeYears = [];
        const currentDate = new Date();
        const currentYear = currentDate.getFullYear();
        for (let i = currentYear; i >= startingYear; i--) {
          activeYears.push(i);
        }

        setActiveYears(activeYears);
      } catch (error) {
        console.error("Error fetching active years:", error);
      }
    };

    fetchActiveYears();
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
