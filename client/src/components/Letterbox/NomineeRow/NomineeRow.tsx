import heart from "../../../assets/svg/heart.png";
import Movie from "../../../types/Movie";
import MediaCard from "../../Common/MediaCard/MediaCard";
import styles from "./NomineeRow.module.css";

interface NomineeRowProps {
  movies: Movie[];
  year: number | undefined;
  movieOnClick: (m: Movie) => void;
  movieEditable: boolean;
  movieOnClickEdit: (m: Movie) => void;
}

const NomineeRow: React.FC<NomineeRowProps> = ({
  movies,
  year,
  movieOnClick,
  movieEditable,
  movieOnClickEdit,
}) => {
  const renderContent = () => {
    return (
      <div className={styles.nomineeRow}>
        <span className={styles.title}>
          <h5>{`${year}`}</h5>
          <img className={styles.heartIcon} src={heart} />
        </span>
        <hr />
        <div className={styles.movieRow}>
          {movies.map((movie) => (
            <MediaCard
              key={movie.id}
              imageUrl={movie.altImageUrl ?? movie.imageUrl}
              title={movie.alias ?? movie.name}
              highlighted={movie.isKino}
              onClick={() => movieOnClick(movie)}
              editable={movieEditable}
              onClickEdit={() => movieOnClickEdit(movie)}
            />
          ))}
        </div>
      </div>
    );
  };

  return renderContent();
};

export default NomineeRow;
