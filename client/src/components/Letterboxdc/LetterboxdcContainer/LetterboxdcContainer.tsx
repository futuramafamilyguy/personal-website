import { useState } from "react";

import { YearProvider } from "../../../contexts/YearContext";
import ActiveYearsContainer from "../ActiveYearsContainer/ActiveYearsContainer";
import PictureGallery from "../PictureGallery/PictureGallery";
import styles from "./LetterboxdcContainer.module.css";

const LetterboxdcContainer: React.FC = () => {
  const [isCollapsed, setIsCollapsed] = useState(false);

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const renderContent = () => {
    return (
      <div className={styles.letterboxdcContainer}>
        <button className={styles.collapseButton} onClick={toggleCollapse}>
          {isCollapsed ? "expand" : "collapse"}
        </button>
        <YearProvider>
          <div
            className={
              isCollapsed
                ? styles.collapsedDescriptionBox
                : styles.descriptionBox
            }
          >
            <h3>letterboxdc (dc for dodging copyright)</h3>
            <h5 className={styles.itsAGoodDayToDance}>
              i'd like to dedicate it to a young man who doesn't think he's seen
              anything good today - cameron frye, this one's for you
            </h5>
            <ActiveYearsContainer />
          </div>
          <PictureGallery />
        </YearProvider>
      </div>
    );
  };

  return renderContent();
};

export default LetterboxdcContainer;
