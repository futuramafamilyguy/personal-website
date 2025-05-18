import React from "react";

import styles from "./NewPostCard.module.css";

interface NewPostCardProps {
  onClick: () => void;
}

const NewPostCard: React.FC<NewPostCardProps> = ({ onClick }) => {
  return (
    <div className={styles.newPostCard} onClick={onClick}>
      <img
        src={"https://cdn.allenmaygibson.com/images/static/lofi-hiphop-mix.jpg"}
        alt={"writing"}
        className={styles.image}
      />
      <div className={styles.titleOverlay} />
    </div>
  );
};

export default NewPostCard;
