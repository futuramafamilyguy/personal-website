import React, { useEffect } from "react";

import {
  debouncedDisableTracking,
  makeDebouncedRequest,
} from "../../personalWebsiteApi";

const DisableTracking: React.FC = () => {
  useEffect(() => {
    const disableTracking = () => {
      makeDebouncedRequest(debouncedDisableTracking, {
        url: `/stats/disable-tracking`,
      }).catch((error: any) => {
        console.error("Error disabling tracking:", error);
      });
    };

    disableTracking();
  }, []);

  return null;
};

export default DisableTracking;
