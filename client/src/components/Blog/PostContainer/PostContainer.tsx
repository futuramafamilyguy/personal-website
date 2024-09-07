import { useEffect, useRef, useState } from "react";
import Post from "../../../types/Post";
import styles from "./PostContainer.module.css";

interface PostContainerProps {
  post: Post;
  onBackClick: () => void;
}

const PostContainer: React.FC<PostContainerProps> = ({ post, onBackClick }) => {
  const [showLess, setShowLess] = useState(false);
  const [isScrollable, setIsScrollable] = useState(false);
  const textContainerRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const textContainer = textContainerRef.current;

    if (textContainer) {
      // Check if the content is scrollable (has a scrollbar)
      setIsScrollable(textContainer.scrollHeight > textContainer.clientHeight);
    }

    const handleResize = () => {
      if (textContainer) {
        setIsScrollable(
          textContainer.scrollHeight > textContainer.clientHeight
        );
      }
    };
    window.addEventListener("resize", handleResize);

    return () => window.removeEventListener("resize", handleResize);
  }, []);

  const formatDate = (dateInput: Date) => {
    const date =
      typeof dateInput === "string" ? new Date(dateInput) : dateInput;

    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  const toggleShowLess = () => {
    setShowLess(!showLess);
  };

  const renderContent = () => {
    return (
      <div className={styles.postContainer}>
        <button className={styles.backButton} onClick={onBackClick}>
          Back
        </button>
        <button className={styles.hideButton} onClick={toggleShowLess}>
          {showLess ? "Show More" : "Show Less"}
        </button>

        <div
          className={
            showLess ? styles.lessImageContainer : styles.imageContainer
          }
        >
          <img src={post.imageUrl} />
        </div>
        <div
          className={
            showLess
              ? styles.moreTextContainerWrapper
              : styles.textContainerWrapper
          }
        >
          <div
            ref={textContainerRef}
            className={
              isScrollable
                ? styles.scrollableTextContainer
                : styles.textContainer
            }
          >
            <div className={styles.metadataContainer}>
              <h3>{post.title}</h3>
              <p>{formatDate(post.createdAtUtc)}</p>
            </div>
            <div className={styles.contentContainer}></div>
          </div>
        </div>
      </div>
    );
  };

  return renderContent();
};

export default PostContainer;
