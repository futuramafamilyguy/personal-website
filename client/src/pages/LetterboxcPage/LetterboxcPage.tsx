import LetterboxcContainer from "../../components/Letterboxc/LetterboxcContainer/LetterboxcContainer.tsx";
import styles from "./LetterboxcPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.letterboxcPage}>
        <LetterboxcContainer />
      </div>
    </>
  );
}

export default LetterboxcPage;
