import { useState } from "react";

import { RegionProvider } from "../../../contexts/RegionContext";
import RegionsContainer from "../RegionsContainer/RegionsContainer";
import SessionContainer from "../SessionGallery/SessionGallery";
import styles from "./SessionHubContainer.module.css";

const SessionsContainer: React.FC = () => {
  const [isCollapsed, setIsCollapsed] = useState(false);

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const calculateNextRun = () => {
    const now = new Date();
    const nextSunday = new Date(now);

    nextSunday.setDate(now.getDate() + ((7 - now.getDay()) % 7));
    nextSunday.setHours(12, 0, 0, 0);

    const difference = nextSunday.getTime() - now.getTime();

    const days = Math.floor(difference / (1000 * 60 * 60 * 24));
    const hours = Math.floor(
      (difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
    );

    if (days === 0 && hours === 0) {
      return `less than an hour`;
    } else if (days === 0) {
      return `${hours} hour${hours > 1 ? "s" : ""}`;
    } else if (days === -1) {
      return "7 days";
    } else {
      return `${days} day${days > 1 ? "s" : ""}`;
    }
  };

  const renderContent = () => {
    return (
      <div className={styles.sessionHubContainer}>
        <button className={styles.collapseButton} onClick={toggleCollapse}>
          {isCollapsed ? "Expand" : "Collapse"}
        </button>
        <RegionProvider>
          <div
            className={
              isCollapsed
                ? styles.collapsedDescriptionBox
                : styles.descriptionBox
            }
          >
            <h3>Session Hub</h3>
            <br />
            <h5>
              Latest pictures showing in NZ cinemas this week, including
              re-releases Updated every Sunday at 12pm NZT (next run in{" "}
              <b>{calculateNextRun()}</b>)
            </h5>
            <RegionsContainer />
          </div>
          <SessionContainer />
        </RegionProvider>
      </div>
    );
  };

  return renderContent();
};

export default SessionsContainer;
