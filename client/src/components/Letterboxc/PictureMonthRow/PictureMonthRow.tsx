import Picture from "../../../types/Picture";
import MediaCard from "../../MediaCard/MediaCard";
import styles from "./PictureMonthRow.module.css";

interface PictureMonthRowProps {
  pictures: Picture[];
  month: string;
  Icon?: React.ReactNode;
  pictureOnClick: (p: Picture) => void;
  pictureEditable: boolean;
  pictureOnClickEdit: () => void;
}

const PictureMonthRow: React.FC<PictureMonthRowProps> = ({
  pictures,
  month,
  Icon,
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
        {Icon && <span style={{ marginLeft: "8px" }}>{Icon}</span>}
        <hr />
        <div className={styles.pictureRow}>
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

export default PictureMonthRow;
