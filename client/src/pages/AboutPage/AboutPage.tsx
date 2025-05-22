import styles from "./AboutPage.module.css";

function AboutPage() {
  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <div className={styles.descriptionBox}>
          <h3>kia ora *~*</h3>
          <h5>
            i'm a frequent moviegoer based in south auckland, new zealand—home
            of big ben pies and lucy lawless xena: warrior princess{" "}
            <span className={styles.smallText}>i love u lucy</span> thanks for
            visiting *~*
          </h5>
          <h5>
            <i>
              i was watching you out there before. i've never seen you look so
              sexy
            </i>
          </h5>
        </div>
      </div>
    </div>
  );
}

export default AboutPage;
