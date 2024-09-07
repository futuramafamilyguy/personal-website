import BlogContainer from "../../components/Blog/BlogContainer/BlogContainer";
import styles from "./BlogPage.module.css";

function BlogPage() {
  return (
    <>
      <div className={styles.blogPage}>
        <BlogContainer />
      </div>
    </>
  );
}

export default BlogPage;
