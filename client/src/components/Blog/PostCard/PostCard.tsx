import React from "react";
import editIcon from "../../../assets/svg/edit_icon.svg";
import styles from "./PostCard.module.css";

interface PostCardProps {
  imageUrl: string;
  title: string;
  onClick: () => void;
  editable: boolean;
  onClickEdit: () => void;
}

const PostCard: React.FC<PostCardProps> = ({
  imageUrl,
  title,
  onClick,
  editable,
  onClickEdit,
}) => {
  return (
    <div className={styles.postCard} onClick={onClick}>
      <img src={imageUrl} alt={title} className={styles.image} />
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
      <div className={styles.titleOverlay}>{title}</div>
    </div>
  );
};

export default PostCard;
