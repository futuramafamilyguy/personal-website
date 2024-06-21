import face from "../../assets/face.png";
import styles from "./AboutPage.module.css";

function AboutPage() {
  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <img src={face} alt="me" className={styles.face} />
        <div className={styles.introBox}>
          <h5>Kia ora, Allen here!</h5>
          <p>
            Welcome to my website where I log movies I watch throughout the year
            and blog about my hot takes because I'm entitled to my opinions
            which of course means I must share them with the world.
          </p>
          <p>
            Thanks for visiting and enjoy my hotter than Taylor Swift takes.
          </p>
          <p>
            As a side note, I will not refer to movies as films so as to not
            sound pretentious and instead use much more down to earth terms like
            pictures.
          </p>
        </div>
      </div>
    </div>
  );
}

export default AboutPage;
