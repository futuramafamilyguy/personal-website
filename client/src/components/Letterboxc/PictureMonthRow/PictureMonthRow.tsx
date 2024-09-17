import Picture from "../../../types/Picture";
import MediaCard from "../../MediaCard/MediaCard";
import styles from "./PictureMonthRow.module.css";

interface PictureMonthRowProps {
  pictures: Picture[];
  month: string;
  pictureOnClick: (p: Picture) => void;
  pictureEditable: boolean;
  pictureOnClickEdit: (p: Picture) => void;
}

const PictureMonthRow: React.FC<PictureMonthRowProps> = ({
  pictures,
  month,
  pictureOnClick,
  pictureEditable,
  pictureOnClickEdit,
}) => {
  const renderContent = () => {
    return (
      <div
        className={
          month !== "January"
            ? styles.pictureMonthRow
            : styles.lastPictureMonthRow
        }
      >
        <span>
          <h5>{month}</h5>
        </span>
        <hr />
        <div className={styles.pictureRow}>
          {pictures.map((picture) => (
            <MediaCard
              key={picture.id}
              imageUrl={picture.imageUrl}
              title={picture.alias ?? picture.name}
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

export default PictureMonthRow;
