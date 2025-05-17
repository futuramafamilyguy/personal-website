import LetterboxdcContainer from "../../components/Letterboxdc/LetterboxdcContainer/LetterboxdcContainer.tsx";
import styles from "./LetterboxdcPage.module.css";

function LetterboxdcPage() {
  return (
    <>
      <div className={styles.letterboxdcPage}>
        <LetterboxdcContainer />
      </div>
    </>
  );
}

export default LetterboxdcPage;
