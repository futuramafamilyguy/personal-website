import ButtonContainer from "../../components/ButtonContainer/ButtonContainer.tsx";
import LetterboxcContainer from "../../components/Letterboxc/LetterboxcContainer/LetterboxcContainer.tsx";
import PictureContainer from "../../components/PictureContainer/PictureContainer";
import { ViewFavoriteProvider } from "../../contexts/ViewFavoriteContext.tsx";
import { YearProvider } from "../../contexts/YearContext.tsx";
import styles from "./LetterboxcPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.letterboxcPage}>
        {/* <div className={styles.descriptionBox}>
          <h4>Letterboxc (c to avoid copyright)</h4>
          <br />
          <p>Pictures I've seen at the cinemas over the past few years.</p>
        </div>
        <YearProvider>
          <ViewFavoriteProvider>
            <ButtonContainer />
            <PictureContainer />
          </ViewFavoriteProvider>
        </YearProvider> */}
        <LetterboxcContainer />
      </div>
    </>
  );
}

export default LetterboxcPage;
