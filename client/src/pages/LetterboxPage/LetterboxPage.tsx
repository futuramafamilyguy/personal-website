import LetterboxContainer from "../../components/Letterbox/LetterboxContainer/LetterboxContainer.tsx";

import styles from "./LetterboxPage.module.css";

function LetterboxPage() {
  return (
    <>
      <div className={styles.letterboxPage}>
        <LetterboxContainer />
      </div>
    </>
  );
}

export default LetterboxPage;
