import axios, { AxiosResponse } from "axios";
import { useEffect, useRef, useState } from "react";
import ReactMarkdown from "react-markdown";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import rehypeRaw from "rehype-raw";

import {
  debouncedFetchPostBySlug,
  makeDebouncedRequest,
} from "../../../api/debouncedFetch";
import { useIsMobile } from "../../../hooks/useIsMobile";
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
              <p>
                {post.isPublished
                  ? formatDate(post.publishedAtUtc)
                  : formatDate(post.createdAtUtc)}
              </p>
            </div>
            <div className={styles.markdownContainer}>
              <ReactMarkdown
                children={markdownContent}
                rehypePlugins={[rehypeRaw]}
                components={{
                  div: ({ className, ...props }) => {
                    if (className === "book") {
                      return <div className={styles.book} {...props} />;
                    } else if (className === "bookImages") {
                      return <div className={styles.bookImages} {...props} />;
                    }

                    return <div className={className} {...props} />;
                  },

                  img: ({ alt, ...props }) => {
                    let className;
                    let style: React.CSSProperties = {};

                    if (alt === "book") {
                      className = styles.embeddedBookImage;
                    } else if (alt!.startsWith("landscape")) {
                      className = styles.embeddedLandscapeImage;

                      const parts = alt!.split("-");

                      const width =
                        parts[1] && !isNaN(Number(parts[1]))
                          ? parts[1]
                          : undefined;
                      const alignment = width ? parts[2] : parts[1];

                      if (width) {
                        style.width = `${width}px`;
                      }

                      switch (alignment) {
                        case "left":
                          style.marginLeft = 0;
                          style.marginRight = "auto";
                          break;

                        case "right":
                          style.marginLeft = "auto";
                          style.marginRight = 0;
                          break;
                      }
                    } else if (alt!.startsWith("portrait")) {
                      // portrait-500 means portrait styling with 500px height
                      className = styles.embeddedPortraitImage;

                      const parts = alt!.split("-");

                      const height =
                        parts[1] && !isNaN(Number(parts[1]))
                          ? parts[1]
                          : undefined;
                      const alignment = height ? parts[2] : parts[1];

                      if (height) {
                        style.height = `${height}px`;
                      }

                      switch (alignment) {
                        case "left":
                          style.marginLeft = 0;
                          style.marginRight = "auto";
                          break;

                        case "right":
                          style.marginLeft = "auto";
                          style.marginRight = 0;
                          break;
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
