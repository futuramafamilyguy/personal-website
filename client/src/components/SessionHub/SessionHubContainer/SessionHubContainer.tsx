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
    const currentDay = now.getDay();
    const currentHour = now.getHours();
    const currentMinute = now.getMinutes();

    let daysUntilSunday = (7 - currentDay) % 7;

    if (currentDay === 0 && currentHour >= 12) {
      daysUntilSunday = 7;
    }

    if (daysUntilSunday > 1) {
      return `${daysUntilSunday} days`;
    } else if (daysUntilSunday === 1) {
      if (currentHour < 12) {
        return "1 day";
      } else {
        const hoursRemaining = 24 - (currentHour - 12);
        return `${hoursRemaining} hours`;
      }
    } else {
      if (currentHour < 11) {
        const hoursRemaining = 12 - currentHour;
        return `${hoursRemaining} hours`;
      } else if (currentHour === 11) {
        if (currentMinute < 59) {
          return "less than an hour";
        } else {
          return "7 days";
        }
      }
    }

    return "7 days";
  };

  const renderContent = () => {
    return (
      <div className={styles.sessionHubContainer}>
        <button className={styles.collapseButton} onClick={toggleCollapse}>
          {isCollapsed ? "expand" : "collapse"}
        </button>
        <RegionProvider>
          <div
            className={
              isCollapsed
                ? styles.collapsedDescriptionBox
                : styles.descriptionBox
            }
          >
            <h3>session hub</h3>
            <br />
            <h5>
              latest pictures showing in nz cinemas this week, including
              re-releases updated every sunday at 12pm nzt (next run in{" "}
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
