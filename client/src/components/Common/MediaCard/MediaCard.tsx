import React from "react";

import editIcon from "../../../assets/svg/edit_icon.svg";
import styles from "./MediaCard.module.css";

interface MediaCardProps {
  imageUrl: string;
  title: string;
  highlighted: boolean;
  onClick: () => void;
  editable: boolean;
  onClickEdit: () => void;
}

const MediaCard: React.FC<MediaCardProps> = ({
  imageUrl,
  title,
  highlighted,
  onClick,
  editable,
  onClickEdit,
}) => {
  return (
    <div
      className={styles.mediaCard}
      onClick={onClick}
      style={
        highlighted
          ? { borderStyle: "solid", borderColor: "#e3bf46" }
          : undefined
      }
    >
      <img src={imageUrl} alt={title} className={styles.mediaImage} />
      {editable ? (
        <div className={styles.edit}>
          <img
            className={styles.editIcon}
            src={editIcon}
            alt="mySvgImage"
            onClick={(e) => {
              e.stopPropagation();
              onClickEdit();
            }}
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
