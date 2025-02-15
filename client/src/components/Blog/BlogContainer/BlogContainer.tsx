import PostGallery from "../PostGallery/PostGallery";
import styles from "./BlogContainer.module.css";

const BlogContainer: React.FC = () => {
  const renderContent = () => {
    return (
      <div className={styles.blogContainer}>
        <div className={styles.postGalleryView}>
          <div className={styles.descriptionBox}>
            <h3>Blog</h3>
            <br />
            <h5>The hottest media takes on the web</h5>
          </div>

          <PostGallery />
        </div>
      </div>
    );
  };

  return renderContent();
};

export default BlogContainer;
