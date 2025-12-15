import Movie from "../../../types/Movie";
import MediaCard from "../../Common/MediaCard/MediaCard";
import styles from "./MonthRow.module.css";

interface MonthRowProps {
  movies: Movie[];
  month: string;
  movieOnClick: (m: Movie) => void;
  movieEditable: boolean;
  movieOnClickEdit: (m: Movie) => void;
}

const MonthRow: React.FC<MonthRowProps> = ({
  movies,
  month,
  movieOnClick,
  movieEditable,
  movieOnClickEdit,
}) => {
  const renderContent = () => {
    return (
      <div className={styles.monthRow}>
        <span>
          <h5>{month}</h5>
        </span>
        <hr />
        <div className={styles.movieRow}>
          {movies.map((movie) => (
            <MediaCard
              key={movie.id}
              imageUrl={movie.imageUrl}
              title={movie.alias ?? movie.name}
              highlighted={movie.isKino}
              onClick={() => movieOnClick(movie)}
              editable={movieEditable}
              onClickEdit={() => movieOnClickEdit(movie)}
              motif={movie.motif}
            />
          ))}
        </div>
      </div>
    );
  };

  return renderContent();
};

export default MonthRow;
