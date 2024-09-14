import { useState } from "react";
import { YearProvider } from "../../../contexts/YearContext";
import ActiveYearsContainer from "../ActiveYearsContainer/ActiveYearsContainer";
import PictureGallery from "../PictureGallery/PictureGallery";
import styles from "./LetterboxcContainer.module.css";

const LetterboxcContainer: React.FC = () => {
  const [isCollapsed, setIsCollapsed] = useState(false);

  const toggleCollapse = () => {
    setIsCollapsed(!isCollapsed);
  };

  const renderContent = () => {
    return (
      <div className={styles.letterboxcContainer}>
        <button className={styles.collapseButton} onClick={toggleCollapse}>
          {isCollapsed ? "Expand" : "Collapse"}
        </button>
        <YearProvider>
          <div
            className={
              isCollapsed
                ? styles.collapsedDescriptionBox
                : styles.descriptionBox
            }
          >
            <h3>Letterboxc (c for avoiding Copyright)</h3>
            <br />
            <h5>Pictures I've seen at the cinemas over the past few years</h5>
            <ActiveYearsContainer />
          </div>
          <PictureGallery />
        </YearProvider>
      </div>
    );
  };

  return renderContent();
};

export default LetterboxcContainer;
