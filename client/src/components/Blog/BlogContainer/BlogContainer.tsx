import { useState } from "react";

import PostGallery from "../PostGallery/PostGallery";
import styles from "./BlogContainer.module.css";
import Post from "../../../types/Post";
import PostContainer from "../PostContainer/PostContainer";

const BlogContainer: React.FC = () => {
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);

  const handlePostClick = (post: Post) => {
    setSelectedPost(post);
  };

  const handleBackClick = () => {
    setSelectedPost(null);
  };

  const renderContent = () => {
    return (
      <div className={styles.blogContainer}>
        {selectedPost ? (
          <PostContainer post={selectedPost} onBackClick={handleBackClick} />
        ) : (
          <div className={styles.postGalleryView}>
            <div className={styles.descriptionBox}>
              <h3>Blog</h3>
              <br />
              <h5>The hottest media takes on the web</h5>
            </div>

            <PostGallery onPostClick={handlePostClick} />
          </div>
        )}
      </div>
    );
  };

  return renderContent();
};

export default BlogContainer;
