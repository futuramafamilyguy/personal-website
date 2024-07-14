import { AxiosResponse } from "axios";
import React, { useEffect, useState } from "react";

import { useRegion, useRegionUpdate } from "../../contexts/RegionContext";
import { debouncedFetchRegions, makeDebouncedRequest } from "../../sessionsApi";
import CapsuleButton from "../CapsuleButton/CapsuleButton";
import styles from "./RegionsContainer.module.css";

const RegionsContainer: React.FC = () => {
  const region = useRegion();
  const updateRegion = useRegionUpdate();
  const [regions, setRegions] = useState<string[]>([]);

  useEffect(() => {
    const fetchRegions = () => {
      makeDebouncedRequest(debouncedFetchRegions, {
        url: `/regions`,
      })
        .then((response: AxiosResponse<string[]>) => {
          const defaultRegion = "Auckland";
          const regions = [];
          regions.push(defaultRegion);
          response.data.forEach((r) => {
            if (r != defaultRegion) {
              regions.push(r);
            }
          });

          setRegions(regions);
        })
        .catch((error: any) => {
          console.error("Error fetching regions:", error);
        });
    };

    fetchRegions();
  }, []);

  if (regions.length === 0) {
    return null;
  }

  return (
    <div className={styles.buttonArea}>
      {regions.map((r) => (
        <CapsuleButton
          key={r}
          text={r}
          onClick={() => updateRegion(r)}
          disabled={false}
          selected={region === r}
        />
      ))}
    </div>
  );
};

export default RegionsContainer;
