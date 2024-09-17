import SessionsContainer from "../../components/Sessions/SessionsContainer/SessionsContainer";
import styles from "./SessionsPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.sessionsPage}>
        <SessionsContainer />
      </div>
    </>
  );
}

export default LetterboxcPage;
