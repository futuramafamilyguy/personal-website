import React from "react";

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
    { name: "Brisbane Central", code: "brisbane-central" },
  ];

  if (regions.length === 0) {
    return null;
  }

  return (
    <div className={styles.buttonArea}>
      {regions.map((r) => (
        <CapsuleButton
          key={r.name}
          text={r.name}
          onClick={() => updateRegion(r.code)}
          disabled={false}
          selected={region === r.code}
        />
      ))}
    </div>
  );
};

export default RegionsContainer;
