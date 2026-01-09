import React from "react";

import { useRegion, useRegionUpdate } from "../../../contexts/RegionContext";
import styles from "./MobileRegionsContainer.module.css";
import Region from "../../../types/Region";

const MobileRegionsContainer: React.FC = () => {
  const region = useRegion();
  const updateRegion = useRegionUpdate();
  const regions: Region[] = [
    { name: "Auckland", code: "auckland" },
    { name: "Canterbury", code: "canterbury" },
    { name: "Brisbane", code: "brisbane-central" },
  ];

  if (regions.length === 0) {
    return null;
  }

  return (
    <div className={styles.buttonArea}>
      {regions.map((r) => (
        <span
          key={r.code}
          className={`${styles.regionText} ${
            region === r.code ? styles.selected : ""
          }`}
          onClick={() => updateRegion(r.code)}
        >
          {r.name}
        </span>
      ))}
    </div>
  );
};

export default MobileRegionsContainer;
