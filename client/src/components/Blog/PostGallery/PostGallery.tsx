import { useState } from "react";

import megamind from "../../../assets/megamind.png";
import { useAuth } from "../../../contexts/AuthContext";
import usePosts from "../../../hooks/usePosts";
import Post from "../../../types/Post";
import MessageDisplay from "../../MessageDisplay/MessageDisplay";
import NewPostCard from "../NewPostCard/NewPostCard";
import NewPostModal from "../NewPostModal/NewPostModal";
import PostCard from "../PostCard/PostCard";
import styles from "./PostGallery.module.css";

interface PostGalleryProps {
  onPostClick: (post: Post) => void;
}

const PostGallery: React.FC<PostGalleryProps> = ({ onPostClick }) => {
  const { posts, loading, setTrigger } = usePosts();
  const isLoggedIn = useAuth();
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);
  const [newModalOpen, setNewModalOpen] = useState(false);

  const openNewPostModal = (post: Post | null) => {
    setSelectedPost(post);
    setNewModalOpen(true);
  };

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (posts.length === 0 && !isLoggedIn) {
        return (
          <MessageDisplay
            message={"no posts?"}
            imageUrl={megamind}
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
                onClick={() => onPostClick(post)}
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
