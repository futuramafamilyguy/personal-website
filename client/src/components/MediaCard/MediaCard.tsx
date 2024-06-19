import React from "react";

import styles from "./MediaCard.module.css";

interface MediaCardProps {
  imageUrl: string;
  title: string;
  onClick: () => void;
}

const MediaCard: React.FC<MediaCardProps> = ({ imageUrl, title, onClick }) => {
  return (
    <div className={styles.mediaCard} onClick={onClick}>
      <img src={imageUrl} alt={title} className={styles.mediaImage} />
      <div className={styles.mediaTitle}>
        <div>
          <p>{title}</p>
        </div>
      </div>
    </div>
  );
};

export default MediaCard;
