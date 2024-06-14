import PictureContainer from "../../components/PictureContainer/PictureContainer";
import styles from "./LetterboxcPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.letterboxcPage}>
        <div className={styles.descriptionBox}>
          <h4>Letterboxc (c to avoid copyright)</h4>
          <br />
          <p>Pictures I've seen at the cinemas over the past few years.</p>
        </div>
        <PictureContainer />
      </div>
    </>
  );
}

export default LetterboxcPage;
