import React, { useState } from "react";

import { useRegion, useRegionUpdate } from "../../../contexts/RegionContext";
import CapsuleButton from "../../Common/CapsuleButton/CapsuleButton";
import styles from "./RegionsContainer.module.css";
import Region from "../../../types/Region";

const RegionsContainer: React.FC = () => {
  const region = useRegion();
  const updateRegion = useRegionUpdate();
  const regions: Region[] = [
    { name: "Auckland", code: "auckland" },
    { name: "Canterbury", code: "canterbury" },
    { name: "Brisbane", code: "brisbane-central" },
  ];

  const [startIndex, setStartIndex] = useState(0);
  const maxVisible = 3;

  const canGoLeft = startIndex > 0;
  const canGoRight = startIndex + maxVisible < regions.length;

  const handleLeft = () => {
    if (canGoLeft) setStartIndex(startIndex - 1);
  };

  const handleRight = () => {
    if (canGoRight) setStartIndex(startIndex + 1);
  };

  const visibleRegions = regions.slice(startIndex, startIndex + maxVisible);

  if (regions.length === 0) {
    return null;
  }

  return (
    <div className={styles.buttonArea}>
      {regions.length > maxVisible && (
        <button
          onClick={handleLeft}
          disabled={!canGoLeft}
          className={styles.arrow}
        >
          {"<"}
        </button>
      )}
      {visibleRegions.map((r) => (
        <CapsuleButton
          key={r.name}
          text={r.name}
          onClick={() => updateRegion(r.code)}
          disabled={false}
          selected={region === r.code}
          width={"125px"}
        />
      ))}
      {regions.length > maxVisible && (
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

export default RegionsContainer;
