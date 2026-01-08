import { useState } from "react";

import { YearFromUrlBridge, YearProvider } from "../../../contexts/YearContext";
import ActiveYearsContainer from "../ActiveYearsContainer/ActiveYearsContainer";
import MovieGallery from "../MovieGallery/MovieGallery";
import styles from "./LetterboxContainer.module.css";
import { useIsMobile } from "../../../hooks/useIsMobile";
import MobileActiveYearsContainer from "../MobileActiveYearsContainer/MobileActiveYearsContainer";

const LetterboxContainer: React.FC = () => {
  const [isCollapsed, setIsCollapsed] = useState(false);
  const isMobile = useIsMobile();

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const renderContent = () => {
    return (
      <div className={styles.letterboxContainer}>
        <button className={styles.collapseButton} onClick={toggleCollapse}>
          {isCollapsed ? "expand" : "collapse"}
        </button>
        <YearProvider>
          <YearFromUrlBridge />
          <div
            className={
              isCollapsed
                ? styles.collapsedDescriptionBox
                : styles.descriptionBox
            }
          >
            <h3>letterbox</h3>
            <h5 className={styles.itsAGoodDayToDance}>
              i'd like to dedicate it to a young man who doesn't think he's seen
              anything good today - cameron frye, this one's for you
            </h5>
          </div>
          {isMobile ? <MobileActiveYearsContainer /> : <ActiveYearsContainer />}
          <MovieGallery />
        </YearProvider>
      </div>
    );
  };

  return renderContent();
};

export default LetterboxContainer;
