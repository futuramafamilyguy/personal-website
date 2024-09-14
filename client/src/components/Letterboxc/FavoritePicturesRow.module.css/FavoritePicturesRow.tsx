import Picture from "../../../types/Picture";
import MediaCard from "../../MediaCard/MediaCard";
import styles from "./FavoritePicturesRow.module.css";
import heartIcon from "../../../assets/svg/heart.svg";

interface FavoritePicturesRowProps {
  pictures: Picture[];
  year: number | undefined;
  pictureOnClick: (p: Picture) => void;
  pictureEditable: boolean;
  pictureOnClickEdit: () => void;
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
          <h5>{`Favorites of ${year}`}</h5>
          <img className={styles.heartIcon} src={heartIcon} />
        </span>
        <hr />
        <div className={styles.pictureRow}>
          {pictures.length === 0 ? <div className={styles.oof}>oof</div> : null}
          {pictures.map((picture, index) => (
            <MediaCard
              key={picture.id}
              imageUrl={picture.imageUrl}
              title={picture.alias ?? picture.name}
              onClick={() => pictureOnClick(picture)}
              editable={pictureEditable}
              onClickEdit={pictureOnClickEdit}
            />
          ))}
        </div>
      </div>
    );
  };

  return renderContent();
};

export default FavoritePicturesRow;
