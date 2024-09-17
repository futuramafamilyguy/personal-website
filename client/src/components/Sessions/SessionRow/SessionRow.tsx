import styles from "./SessionRow.module.css";
import { Session } from "../../../types/Session";
import MediaCard from "../../MediaCard/MediaCard";

interface SessionRowProps {
  sessions: Session[];
  category: string;
  sessionOnClick: (s: Session) => void;
}

const SessionRow: React.FC<SessionRowProps> = ({
  sessions,
  category,
  sessionOnClick,
}) => {
  const renderContent = () => {
    return (
      <div
        className={
          category === "New Releases"
            ? styles.sessionRow
            : styles.lastSessionRow
        }
      >
        <span>
          <h5>{category}</h5>
        </span>
        <hr />
        <div className={styles.sessionContainer}>
          {sessions.map((session) => (
            <MediaCard
              key={session.movie.name}
              imageUrl={session.movie.imageUrl}
              title={session.movie.name}
              onClick={() => sessionOnClick(session)}
              editable={false}
              onClickEdit={() => {}}
            />
          ))}
        </div>
      </div>
    );
  };

  return renderContent();
};

export default SessionRow;