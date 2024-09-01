import React from "react";

import styles from "./PostCard.module.css";

interface PostCardProps {
  imageUrl: string;
  title: string;
  onClick: () => void;
}

const PostCard: React.FC<PostCardProps> = ({ imageUrl, title, onClick }) => {
  return (
    <div className={styles.postCard}>
      <img
        src={imageUrl}
        alt={title}
        className={styles.image}
        onClick={onClick}
      />
      <div className={styles.titleOverlay}>{title}</div>
    </div>
  );
};

export default PostCard;
