import React from "react";

import styles from "./NewMediaCard.module.css";

interface NewMediaCardProps {
  imageSrc: string;
  onClick: () => void;
}

const NewMediaCard: React.FC<NewMediaCardProps> = ({ imageSrc, onClick }) => {
  return (
    <div className={styles.newMediaCard} onClick={onClick}>
      <img src={imageSrc} alt="where imag" className={styles.image} />
    </div>
  );
};

export default NewMediaCard;
