import editIcon from "../../../assets/svg/edit_icon.svg";
import useIntersectionObserver from "../../../hooks/useIntersectionObserver";
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
  const { isInView, ref } = useIntersectionObserver();

  return (
    <div className={styles.postCard} onClick={onClick} ref={ref}>
      <img
        data-src={imageUrl}
        src={isInView ? imageUrl : ""}
        alt="where imag"
        className={styles.image}
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
      <div className={styles.titleOverlay}>{title}</div>
    </div>
  );
};

export default PostCard;
