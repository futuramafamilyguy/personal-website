import styles from "./AboutPage.module.css";

function AboutPage() {
  return (
    <div className={styles.aboutPage}>
      <div className={styles.centeredContainer}>
        <div className={styles.descriptionBox}>
          <h3>kia ora *~*</h3>
          <h5>
            I'm a frequent moviegoer based in South Auckland, New Zealand—home
            of Big Ben Pies and Lucy Lawless, aka Xena: Warrior Princess{" "}
            <span className={styles.smallText}>i love u lucy</span> Thanks for
            visiting *~*
          </h5>
          <h5>
            <i>life's hard but rewa's harder</i>
          </h5>
        </div>
      </div>
    </div>
  );
}

export default AboutPage;
