import React from "react";

import writing from "../../../assets/newpost.jpg";
import styles from "./NewPostCard.module.css";

interface NewPostCardProps {
  onClick: () => void;
}

const NewPostCard: React.FC<NewPostCardProps> = ({ onClick }) => {
  return (
    <div className={styles.newPostCard} onClick={onClick}>
      <img src={writing} alt={"writing"} className={styles.image} />
      <div className={styles.titleOverlay} />
    </div>
  );
};

export default NewPostCard;
