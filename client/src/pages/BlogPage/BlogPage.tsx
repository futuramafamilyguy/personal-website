import styles from "./BlogPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.blogPage}>
        <div className={styles.descriptionBox}>
          <h4>Blog</h4>
          <br />
          <p>Pictures I've seen at the cinemas over the past few years.</p>
        </div>
      </div>
    </>
  );
}

export default LetterboxcPage;
