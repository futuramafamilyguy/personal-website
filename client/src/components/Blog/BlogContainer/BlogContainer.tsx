import { useIsMobile } from "../../../hooks/useIsMobile";
import PostGallery from "../PostGallery/PostGallery";
import styles from "./BlogContainer.module.css";

const BlogContainer: React.FC = () => {
  const isMobile = useIsMobile();

  const renderContent = () => {
    return (
      <div className={styles.blogContainer}>
        <div className={styles.postGalleryView}>
          <div className={styles.descriptionBox}>
            {isMobile ? null : (
              <>
                <h3>blog</h3>
                <h5 className={styles.itsAGoodDayToDance}>
                  i'm crazy for trying, and crazy for crying, and i'm crazy for
                  loving you
                </h5>
              </>
            )}
          </div>

          <PostGallery />
        </div>
      </div>
    );
  };

  return renderContent();
};

export default BlogContainer;
