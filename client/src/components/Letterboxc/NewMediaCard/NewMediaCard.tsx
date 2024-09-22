import React from "react";

import styles from "./NewMediaCard.module.css";

import goingtocinema from "../../../assets/gotocinema.webp";

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
      <p>New {mediaType}</p>
    </div>
  );
};

export default NewMediaCard;
