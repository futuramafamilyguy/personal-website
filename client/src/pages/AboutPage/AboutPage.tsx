import profileImage from "../../assets/allen-icon.jpeg";
import styles from "./AboutPage.module.css";

function AboutPage() {
  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <img src={profileImage} alt="me" className={styles.profileImage} />
        <div className={styles.introBox}>
          <h5>Kia ora, Allen here.</h5>
          <p className={styles.p}>
            Welcome to my website where I log movies I watch throughout the year
            and blog about my hot takes because I'm entitled to opinions which
            of course means that I must share them with the world.
          </p>
          <p className={styles.p}>
            Thanks for visiting and enjoy my hotter than Taylor Swift takes.
          </p>
          <p className={styles.p}>
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
