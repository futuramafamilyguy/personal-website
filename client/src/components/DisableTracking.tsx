import {
  debouncedDisableTracking,
  makeDebouncedRequest,
} from "../personalWebsiteApi";
import React, { useEffect } from "react";

const DisableTracking: React.FC = () => {
  useEffect(() => {
    const disableTracking = () => {
      makeDebouncedRequest(debouncedDisableTracking, {
        url: "/stats/disable-tracking",
        method: "post",
      }).catch((error: any) => {
        console.error("Error disabling tracking:", error);
      });
    };

    disableTracking();
  }, []);

  return null;
};

export default DisableTracking;
