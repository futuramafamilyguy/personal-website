import megamind from "../../../assets/megamind.png";
import usePosts from "../../../hooks/usePosts";
import Post from "../../../types/Post";
import MessageDisplay from "../../MessageDisplay/MessageDisplay";
import PostCard from "../PostCard/PostCard";
import styles from "./PostGallery.module.css";

interface PostGalleryProps {
  onPostClick: (post: Post) => void;
}

const PostGallery: React.FC<PostGalleryProps> = ({ onPostClick }) => {
  const { posts, loading } = usePosts();

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (posts.length > 0) {
        return (
          <>
            <div className={styles.postGallery}>
              {posts.map((post) => (
                <PostCard
                  key={post.id}
                  imageUrl={post.imageUrl}
                  title={post.title}
                  onClick={() => onPostClick(post)}
                />
              ))}
            </div>
          </>
        );
      } else {
        return (
          <MessageDisplay
            message={"no posts?"}
            imageUrl={megamind}
          ></MessageDisplay>
        );
      }
    }
  };

  return renderContent();
};

export default PostGallery;
