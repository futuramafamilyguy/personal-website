import React from "react";

import styles from "./NewMediaCard.module.css";

interface MediaCardProps {
  mediaType: string;
  onClick: () => void;
}

const NewMediaCard: React.FC<MediaCardProps> = ({ mediaType, onClick }) => {
  return (
    <div className={styles.newMediaCard} onClick={onClick}>
      <p>New {mediaType}</p>
    </div>
  );
};

export default NewMediaCard;
