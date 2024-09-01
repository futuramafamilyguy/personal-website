import PostsContainer from "../../components/PostsContainer/PostsContainer";
import styles from "./BlogPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.blogPage}>
        <div className={styles.contentBox}>
          <div className={styles.descriptionBox}>
            <h3>Blog</h3>
            <br />
            <h5>Hottest media takes on the web</h5>
          </div>
          <PostsContainer />
        </div>
      </div>
    </>
  );
}

export default LetterboxcPage;
