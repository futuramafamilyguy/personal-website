import megamind from "../../assets/megamind.png";
import usePosts from "../../hooks/usePosts";
import MessageDisplay from "../MessageDisplay/MessageDisplay";
import PostCard from "../PostCard/PostCard";
import styles from "./PostsContainer.module.css";

const PostsContainer: React.FC = () => {
  const { posts, loading } = usePosts();

  const renderContent = () => {
    if (loading) {
      return <MessageDisplay message={"Loading..."} />;
    } else {
      if (posts.length > 0) {
        return (
          <>
            <div className={styles.postsContainer}>
              {posts.map((post) => (
                <PostCard
                  key={post.id}
                  imageUrl={post.imageUrl}
                  title={post.title}
                  onClick={() => {}}
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

export default PostsContainer;
