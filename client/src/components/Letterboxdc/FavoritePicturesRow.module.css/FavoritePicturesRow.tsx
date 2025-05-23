import heartIcon from "../../../assets/svg/heart_filled.png";
import Picture from "../../../types/Picture";
import MediaCard from "../../Common/MediaCard/MediaCard";
import styles from "./FavoritePicturesRow.module.css";

interface FavoritePicturesRowProps {
  pictures: Picture[];
  year: number | undefined;
  pictureOnClick: (p: Picture) => void;
  pictureEditable: boolean;
  pictureOnClickEdit: (p: Picture) => void;
}

const FavoritePicturesRow: React.FC<FavoritePicturesRowProps> = ({
  pictures,
  year,
  pictureOnClick,
  pictureEditable,
  pictureOnClickEdit,
}) => {
  const renderContent = () => {
    return (
      <div className={styles.favoritePicturesRow}>
        <span className={styles.title}>
          <h5>{`${year}`}</h5>
          <img className={styles.heartIcon} src={heartIcon} />
        </span>
        <hr />
        <div className={styles.pictureRow}>
          {pictures.map((picture) => (
            <MediaCard
              key={picture.id}
              imageUrl={picture.altImageUrl ?? picture.imageUrl}
              title={picture.alias ?? picture.name}
              highlighted={picture.isKino}
              onClick={() => pictureOnClick(picture)}
              editable={pictureEditable}
              onClickEdit={() => pictureOnClickEdit(picture)}
            />
          ))}
        </div>
      </div>
    );
  };

  return renderContent();
};

export default FavoritePicturesRow;
