import axios, { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import ReactMarkdown from "react-markdown";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import rehypeRaw from "rehype-raw";

import { debouncedFetchPostBySlug, makeDebouncedRequest } from "../../../personalWebsiteApi";
import Post from "../../../types/Post";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import styles from "./PostContainer.module.css";

const PostContainer: React.FC = () => {
  const { state } = useLocation();
  const { slug } = useParams();
  const [post, setPost] = useState(state?.post || null);
  const [markdownContent, setMarkdownContent] = useState<string>("");
  const [showLess, setShowLess] = useState(false);
  const textContainerRef = useRef<HTMLDivElement | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (!post) {
      makeDebouncedRequest(debouncedFetchPostBySlug, {
        url: `/posts/${slug}`,
      })
        .then((response: AxiosResponse<Post>) => {
          setPost(response.data);
        })
        .catch((error: any) => {
          setPost(null);
          navigate("/blog");
        });
    }
  }, [post, slug]);

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
  }, [post]);

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

  const handleBackClick = () => {
    navigate("/blog");
  };

  const renderContent = () => {
    if (!post) {
      return <MessageDisplay message={"Loading..."} />;
    }

    return (
      <div className={styles.postContainer}>
        <button className={styles.backButton} onClick={handleBackClick}>
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
