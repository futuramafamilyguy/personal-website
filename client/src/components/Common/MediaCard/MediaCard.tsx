import React from "react";

import editIcon from "../../../assets/svg/edit_icon.svg";
import useIntersectionObserver from "../../../hooks/useIntersectionObserver";
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
  const { isInView, ref } = useIntersectionObserver();

  return (
    <div
      className={styles.mediaCard}
      onClick={onClick}
      style={
        highlighted
          ? { borderStyle: "solid", borderColor: "#e3bf46" }
          : undefined
      }
      ref={ref}
    >
      <img
        data-src={imageUrl}
        src={isInView ? imageUrl : ""}
        alt="where imag"
        className={styles.mediaImage}
      />
      {editable ? (
        <div className={styles.edit}>
          <img
            className={styles.editIcon}
            src={editIcon}
            alt="where imag"
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
