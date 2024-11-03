import axios from "axios";
import { useEffect, useRef, useState } from "react";
import ReactMarkdown from "react-markdown";
import rehypeRaw from "rehype-raw";

import Post from "../../../types/Post";
import styles from "./PostContainer.module.css";

interface PostContainerProps {
  post: Post;
  onBackClick: () => void;
}

const PostContainer: React.FC<PostContainerProps> = ({ post, onBackClick }) => {
  const [markdownContent, setMarkdownContent] = useState<string>("");
  const [showLess, setShowLess] = useState(false);
  const [isScrollable, setIsScrollable] = useState(false);
  const textContainerRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const fetchMarkdown = async () => {
      try {
        const response = await axios.get(post.contentUrl);
        setMarkdownContent(response.data);
      } catch (error) {
        console.error("Error fetching markdown content:", error);
      }
    };

    fetchMarkdown();
  }, []);

  useEffect(() => {
    // need to wait for markdown content to be loaded first before determining scrollability
    if (markdownContent) {
      const textContainer = textContainerRef.current;

      if (textContainer) {
        setIsScrollable(
          textContainer.scrollHeight > textContainer.clientHeight
        );
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
    }
  }, [markdownContent]);

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
          {showLess ? "Expand" : "Collapse"}
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
          <div ref={textContainerRef} className={styles.textContainer}>
            <div className={styles.metadataContainer}>
              <h3>{post.title}</h3>
              <p>{formatDate(post.createdAtUtc)}</p>
            </div>
            <div className={styles.markdownContainer}>
              <ReactMarkdown
                children={markdownContent}
                rehypePlugins={[rehypeRaw]}
                components={{
                  img: ({ alt, ...props }) => {
                    let className;

                    switch (alt) {
                      case "book":
                        className = styles.embeddedBookImage;
                        break;
                      case "landscape":
                        className = styles.embeddedLandscapeImage;
                        break;
                      case "portrait":
                        className = styles.embeddedPortraitImage;
                        break;
                      default:
                        className = styles.embeddedLandscapeImage;
                    }
                    return (
                      <img
                        className={className}
                        {...props}
                        alt="why isn't the image loading"
                      />
                    );
                  },
                  blockquote: ({ node, ...props }) => (
                    <blockquote className={styles.embeddedQuote} {...props} />
                  ),
                }}
              />
            </div>
          </div>
        </div>
      </div>
    );
  };

  return renderContent();
};

export default PostContainer;
