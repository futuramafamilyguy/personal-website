import React from "react";

import styles from "./NewMediaCard.module.css";

interface NewMediaCardProps {
  onClick: () => void;
}

const NewMediaCard: React.FC<NewMediaCardProps> = ({ onClick }) => {
  return (
    <div className={styles.newMediaCard} onClick={onClick}>
      <img
        src={"https://cdn.allenmaygibson.com/images/static/the-fourth.webp"}
        alt="where imag"
        className={styles.image}
      />
    </div>
  );
};

export default NewMediaCard;
