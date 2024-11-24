import styles from "./AboutPage.module.css";

function AboutPage() {
  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <div className={styles.descriptionBox}>
          <h3>Welcome to my website *~*</h3>
          <h5>
            This is a space for me to log movies I watch throughout the year and
            blog about my hot takes because I'm entitled to my opinions which of
            course means I must share them with the world
          </h5>
          <h5>
            Thanks for visiting and enjoy my hotter than Taylor Swift takes
          </h5>
          <div className={styles.footnote}>
            <p>
              As a side note, I will not refer to movies as films so as to not
              sound pretentious and instead use much more down to earth terms
              like pictures
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AboutPage;
