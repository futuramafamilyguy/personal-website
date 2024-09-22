import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

import notFoundBoy from "../assets/404boy.png";
import notFoundGirl from "../assets/404girl.png";
import { debouncedFetchPosts, makeDebouncedRequest } from "../personalWebsiteApi";
import Post from "../types/Post";

const usePosts = () => {
  const [posts, setPosts] = useState<Post[]>([]);
  const [loading, setLoading] = useState(false);

  const [trigger, setTrigger] = useState(false);

  useEffect(() => {
    const fetchPosts = () => {
      setLoading(true);
      makeDebouncedRequest(debouncedFetchPosts, {
        url: "/posts",
      })
        .then((response: AxiosResponse<Post[]>) => {
          const data = response.data;
          const updatedPosts = (<Post[]>data).map((post) => ({
            ...post,
            imageUrl: setDefaultImageUrl(post.imageUrl),
          }));

          setPosts(updatedPosts);
        })
        .catch((error: any) => {
          setPosts([]);
          console.error("Error fetching posts:", error);
        })
        .finally(() => setLoading(false));
    };

    fetchPosts();
  }, [trigger]);

  const setDefaultImageUrl = (imageUrl: string) => {
    return imageUrl || (Math.random() < 0.5 ? notFoundBoy : notFoundGirl);
  };

  return {
    posts,
    loading,
    setTrigger,
  };
};

export default usePosts;
