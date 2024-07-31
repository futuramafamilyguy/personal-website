import React from "react";

import styles from "./MediaCard.module.css";

import editIcon from "../../assets/svg/edit_icon.svg";

interface MediaCardProps {
  imageUrl: string;
  title: string;
  onClick: () => void;
  editable: boolean;
  onClickEdit: () => void;
}

const MediaCard: React.FC<MediaCardProps> = ({
  imageUrl,
  title,
  onClick,
  editable,
  onClickEdit,
}) => {
  return (
    <div className={styles.mediaCard}>
      <img
        src={imageUrl}
        alt={title}
        className={styles.mediaImage}
        onClick={onClick}
      />
      {editable ? (
        <div className={styles.edit}>
          <img
            className={styles.editIcon}
            src={editIcon}
            alt="mySvgImage"
            onClick={onClickEdit}
          />
        </div>
      ) : null}
      <div className={styles.mediaTitle}>
        <div>
          <p>{title}</p>
        </div>
      </div>
    </div>
  );
};

export default MediaCard;
