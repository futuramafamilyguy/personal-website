import axios, { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import ReactMarkdown from "react-markdown";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import rehypeRaw from "rehype-raw";

import {
  debouncedFetchPostBySlug,
  makeDebouncedRequest,
} from "../../../api/debouncedFetch";
import Post from "../../../types/Post";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import styles from "./PostContainer.module.css";
import { useIsMobile } from "../../../hooks/useIsMobile";

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
          console.error("Error fetching post:", error);
          setPost(null);
          navigate("/blog");
        });
    }
  }, [post, slug]);

  useEffect(() => {
    const fetchMarkdown = async () => {
      try {
        const response = await axios.get(post.markdownUrl);
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

  const isMobile = useIsMobile();

  const renderContent = () => {
    if (!post) {
      return <MessageDisplay message={"loading..."} />;
    }

    return (
      <div className={styles.postContainer}>
        {!isMobile ? (
          <>
            <button className={styles.backButton} onClick={handleBackClick}>
              back
            </button>
            <button className={styles.hideButton} onClick={toggleShowLess}>
              {showLess ? "expand" : "collapse"}
            </button>
          </>
        ) : (
          <div className={styles.backButtonContainer}>
            <button className={styles.backButton} onClick={handleBackClick}>
              back
            </button>
          </div>
        )}

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
                    let style = {};

                    if (alt === "book") {
                      className = styles.embeddedBookImage;
                    } else if (alt === "landscape") {
                      className = styles.embeddedLandscapeImage;
                    } else if (alt!.startsWith("portrait")) {
                      // portrait-500 means portrait styling with 500px height
                      className = styles.embeddedPortraitImage;

                      const portraitHeight = alt!.split("-")[1];
                      if (portraitHeight) {
                        style = { height: `${portraitHeight}px` };
                      }
                    }
                    return (
                      <img
                        className={className}
                        style={style}
                        {...props}
                        alt="where imag"
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
