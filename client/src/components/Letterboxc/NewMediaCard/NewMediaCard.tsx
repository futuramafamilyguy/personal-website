import React from "react";

import goingtocinema from "../../../assets/gotocinema.webp";
import styles from "./NewMediaCard.module.css";

interface NewMediaCardProps {
  mediaType: string;
  onClick: () => void;
}

const NewMediaCard: React.FC<NewMediaCardProps> = ({ mediaType, onClick }) => {
  return (
    <div className={styles.newMediaCard} onClick={onClick}>
      <img
        src={goingtocinema}
        alt={"going to the cinemas"}
        className={styles.image}
      />
      <p>I saw something</p>
    </div>
  );
};

export default NewMediaCard;
