import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { useAuth } from "../../../contexts/AuthContext";
import usePosts from "../../../hooks/usePosts";
import Post from "../../../types/Post";
import MessageDisplay from "../../Common/MessageDisplay/MessageDisplay";
import NewPostCard from "../NewPostCard/NewPostCard";
import NewPostModal from "../NewPostModal/NewPostModal";
import PostCard from "../PostCard/PostCard";
import styles from "./PostGallery.module.css";

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
      );
    }
  };

  return renderContent();
};

export default PostGallery;
