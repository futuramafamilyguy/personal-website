import styles from "./SessionRow.module.css";
import { Session } from "../../../types/Session";
import MediaCard from "../../Common/MediaCard/MediaCard";

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
          category === "new releases"
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
              key={session.title}
              imageUrl={session.imageUrl}
              title={session.title}
              onClick={() => sessionOnClick(session)}
              editable={false}
              onClickEdit={() => {}}
              highlighted={false}
            />
          ))}
        </div>
      </div>
    );
  };

  return renderContent();
};

export default SessionRow;
