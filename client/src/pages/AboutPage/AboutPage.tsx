import profileImage from "../../assets/about-me.jpg";
import styles from "./AboutPage.module.css";

function AboutPage() {
  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <img src={profileImage} alt="me" className={styles.profileImage} />
        <div className={styles.introBox}>
          <h5>Kia ora, Allen here.</h5>
          <br />
          <p>
            Welcome to my website where I log movies I watch throughout the year
            and provide hot takes on them because I'm entitled to opinions which
            must therefore mean my only course of action is to share them with
            the the world.
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