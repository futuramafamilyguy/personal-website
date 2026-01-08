import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";

import { useAuth } from "../../../contexts/AuthContext";
import usePosts from "../../../hooks/usePosts";
import Post from "../../../types/Post";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import NewPostCard from "../NewPostCard/NewPostCard";
import NewPostModal from "../NewPostModal/NewPostModal";
import PostCard from "../PostCard/PostCard";
import styles from "./PostGallery.module.css";
import { useIsMobile } from "../../../hooks/useIsMobile";

const PostGallery: React.FC = () => {
  const { posts, loading, setTrigger } = usePosts();
  const isLoggedIn = useAuth();
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);
  const [newModalOpen, setNewModalOpen] = useState(false);
  const navigate = useNavigate();

  const openNewPostModal = (post: Post | null) => {
    setSelectedPost(post);
    setNewModalOpen(true);
  };

  const handlePostClick = (post: Post) => {
    navigate(`/blog/${post.slug}`, { state: { post } });
    isMobile && window.scrollTo({ top: 0, behavior: "smooth" });
  };

  const isMobile = useIsMobile();

  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollIntoView({
        behavior: "auto", // or "smooth" if you want animation
        block: "center", // vertically center the selected movie
      });
    }
  }, [selectedPost]);

  const formatDate = (dateInput: Date) => {
    const date =
      typeof dateInput === "string" ? new Date(dateInput) : dateInput;

    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"loading..."} />;
    } else {
      if (posts.length === 0 && !isLoggedIn) {
        return (
          <MessageDisplay
            message={"no takes?"}
            imageUrl={
              "https://cdn.allenmaygibson.com/images/static/space-mom.jpg"
            }
          ></MessageDisplay>
        );
      }
      return (
        <>
          {isMobile ? (
            <div className={styles.mobilePostGallery}>
              {posts.map((post) => (
                <div
                  key={post.id}
                  className={styles.postDetails}
                  ref={post.id === selectedPost?.id ? scrollRef : null} // <-- attach ref
                  onClick={() => handlePostClick(post)}
                >
                  <div className={styles.textBlock}>
                    <div className={styles.titleRow}>
                      <h5 className={styles.postTitle}>{post.title}</h5>
                    </div>
                    <div className={styles.date}>
                      {formatDate(post.createdAtUtc)}
                    </div>
                  </div>
                  <div className={styles.imageContainer}>
                    <img
                      src={post?.imageUrl}
                      alt={post.title}
                      className={styles.modalImage}
                      loading="lazy"
                    />
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <>
              <div className={styles.postGallery}>
                {isLoggedIn ? (
                  <NewPostCard onClick={() => openNewPostModal(null)} />
                ) : null}
                {posts.map((post) => (
                  <PostCard
                    key={post.id}
                    imageUrl={post.imageUrl}
                    title={post.title}
                    onClick={() => handlePostClick(post)}
                    editable={isLoggedIn}
                    onClickEdit={() => openNewPostModal(post)}
                  />
                ))}
              </div>
              {isLoggedIn ? (
                <NewPostModal
                  isOpen={newModalOpen}
                  onClose={() => {
                    setNewModalOpen(false);
                    setSelectedPost(null);
                  }}
                  post={selectedPost}
                  setTrigger={() => setTrigger((prevTrigger) => !prevTrigger)}
                />
              ) : null}
            </>
          )}
        </>
      );
    }
  };

  return renderContent();
};

export default PostGallery;
