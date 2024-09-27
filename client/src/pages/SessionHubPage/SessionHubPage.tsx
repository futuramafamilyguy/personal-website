import SessionsContainer from "../../components/SessionHub/SessionHubContainer/SessionHubContainer.tsx";
import styles from "./SessionHubPage.module.css";

function SessionHubPage() {
  return (
    <>
      <div className={styles.sessionsPage}>
        <SessionsContainer />
      </div>
    </>
  );
}

export default SessionHubPage;
