import axios, { AxiosResponse } from "axios";
import { FormEvent, useEffect, useState } from "react";
import ReactDom from "react-dom";

import { debouncedCreatePost, debouncedDeleteContent, debouncedDeleteImage, debouncedDeletePost, debouncedUpdatePost, debouncedUploadContent, debouncedUploadImage, makeDebouncedRequest } from "../../../personalWebsiteApi";
import Post from "../../../types/Post";
import styles from "./NewPostModal.module.css";

interface NewPostModalProps {
  isOpen: boolean;
  onClose: () => void;
  post: Post | null;
  setTrigger: () => void;
}

interface CreatePostResponse {
  id: string;
}

interface UploadImageResponse {
  imageUrl: string;
}

const NewPostModal: React.FC<NewPostModalProps> = ({
  isOpen,
  onClose,
  post,
  setTrigger,
}) => {
  const [postId, setPostId] = useState("");
  const [title, setTitle] = useState("");
  const [image, setImage] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState<string | null>(null);
  const [markdownContent, setMarkdownContent] = useState("");
  const [contentUrl, setContentUrl] = useState<string | null>(null);
  const [createdAtUtc, setCreatedAtUtc] = useState<Date | undefined>(undefined);

  const [result, setResult] = useState("");

  useEffect(() => {
    setResult("");
    setPostId(post ? post.id : "");
    setTitle(post ? post.title : "");
    setImage(null);
    setImageUrl(post && post.imageUrl ? post.imageUrl : "");
    setContentUrl(post && post.contentUrl ? post.contentUrl : "");
    setCreatedAtUtc(post ? post.createdAtUtc : undefined);
  }, [isOpen]);

  useEffect(() => {
    const fetchMarkdown = async () => {
      try {
        const response = await axios.get(post!.contentUrl);
        setMarkdownContent(response.data);
      } catch (error) {
        console.error("Error fetching markdown content:", error);
      }
    };

    setMarkdownContent("");
    if (post && post.contentUrl) {
      fetchMarkdown();
    }
  }, [isOpen]);

  const createPost = () => {
    return makeDebouncedRequest(debouncedCreatePost, {
      url: `/posts`,
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      data: JSON.stringify({
        title: title,
      }),
    });
  };

  const uploadImage = (id: string): Promise<string> => {
    const formData = new FormData();
    formData.append("imageFile", image!);

    return makeDebouncedRequest(debouncedUploadImage, {
      url: `/posts/${id}/image`,
      method: "post",
      headers: {
        "Content-Type": "multipart/form-data",
      },
      data: formData,
    }).then((response) => response.data.imageUrl);
  };

  const uploadContent = (id: string): Promise<string> => {
    return makeDebouncedRequest(debouncedUploadContent, {
      url: `/posts/${id}/content`,
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      data: JSON.stringify({
        content: markdownContent,
      }),
    }).then((response) => response.data.contentUrl);
  };

  const deleteImage = (id: string) => {
    return makeDebouncedRequest(debouncedDeleteImage, {
      url: `/posts/${id}/image`,
      method: "delete",
    });
  };

  const deleteContent = (id: string) => {
    return makeDebouncedRequest(debouncedDeleteContent, {
      url: `/posts/${id}/content`,
      method: "delete",
    });
  };

  const deletePost = (id: string) => {
    return makeDebouncedRequest(debouncedDeletePost, {
      url: `/posts/${id}`,
      method: "delete",
    });
  };

  const updatePost = (
    id: string,
    updatedTitle?: string,
    updatedCreatedAtUtc?: Date,
    updatedImageUrl?: string,
    updatedContentUrl?: string
  ) => {
    return makeDebouncedRequest(debouncedUpdatePost, {
      url: `/posts/${id}`,
      method: "put",
      headers: {
        "Content-Type": "application/json",
      },
      data: JSON.stringify({
        title: updatedTitle ? updatedTitle : title,
        createdAtUtc: updatedCreatedAtUtc ? updatedCreatedAtUtc : createdAtUtc,
        imageUrl: updatedImageUrl
          ? updatedImageUrl
          : imageUrl && !isDefaultImage(imageUrl)
          ? imageUrl
          : null,
        contentUrl: updatedContentUrl ? updatedContentUrl : contentUrl,
      }),
    });
  };

  const orchestrateCreatePost = async () => {
    try {
      const postResponse = await createPost();
      const newPostId = postResponse.data.id;

      const uploadPromises: Promise<string | undefined>[] = [];

      if (image) {
        uploadPromises.push(uploadImage(newPostId));
      } else {
        uploadPromises.push(Promise.resolve(undefined));
      }

      if (markdownContent) {
        uploadPromises.push(uploadContent(newPostId));
      } else {
        uploadPromises.push(Promise.resolve(undefined));
      }

      const [imageResponse, contentResponse] = await Promise.all(
        uploadPromises
      );

      await updatePost(
        newPostId,
        undefined,
        postResponse.data.createdAtUtc,
        imageResponse,
        contentResponse
      );
    } catch (error) {
      console.error("Error creating post:", error);
      setResult("Error creating post");
    }
  };

  const orchestrateUpdatePost = async () => {
    try {
      const updatePromises: Promise<string | undefined>[] = [];

      if (image) {
        if (!isDefaultImage(imageUrl!)) {
          await deleteImage(postId);
        }

        updatePromises.push(uploadImage(postId));
      } else {
        updatePromises.push(Promise.resolve(undefined));
      }

      if (markdownContent) {
        updatePromises.push(uploadContent(postId));
      } else {
        updatePromises.push(Promise.resolve(undefined));
      }

      const [imageResponse, contentResponse] = await Promise.all(
        updatePromises
      );

      await updatePost(
        postId,
        title,
        createdAtUtc,
        imageResponse,
        contentResponse
      );
    } catch (error) {
      console.error("Error updating post:", error);
      setResult("Error updating post");
    }
  };

  const orchestrateDeletePost = async () => {
    try {
      const deletePromises: Promise<any>[] = [];

      if (imageUrl ? !isDefaultImage(imageUrl) : false) {
        deletePromises.push(deleteImage(postId));
      }

      if (markdownContent) {
        deletePromises.push(deleteContent(postId));
      }

      await Promise.all(deletePromises);
      await deletePost(postId);
    } catch (error) {
      console.error("Error deleting post:", error);
      setResult("Error deleting post");
    }
  };

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    if (post) {
      await orchestrateUpdatePost();
    } else {
      await orchestrateCreatePost();
    }

    setTrigger();
    onClose();
  };

  const handleDelete = async (event: FormEvent) => {
    event.preventDefault();
    await orchestrateDeletePost();

    setTrigger();
    onClose();
  };

  const isDefaultImage = (url: string) => url.includes("/assets/404");

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.textContainer}>
          {post ? (
            <h5 className={styles.title}>Update Post</h5>
          ) : (
            <h5 className={styles.title}>Create New Post</h5>
          )}
          <form onSubmit={handleSubmit}>
            <div className={styles.formGroup}>
              <label>Title</label>
              <input
                type="text"
                id="title"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                required
                className={styles.inputField}
              ></input>
            </div>
            <div className={styles.formGroup}>
              <label>Image</label>
              <input
                type="file"
                onChange={(e) =>
                  setImage(e.target.files ? e.target.files[0] : null)
                }
              />
            </div>
            <div className={styles.formGroup}>
              <label>Content</label>
              <textarea
                value={markdownContent}
                onChange={(e) => setMarkdownContent(e.target.value)}
                placeholder="Type your markdown here"
              />
            </div>
            <div className={styles.buttonContainer}>
              <div>
                <button className={styles.button} type="submit">
                  Submit
                </button>
                <span className={styles.result} id="result">
                  {result}
                </span>
              </div>
              {post ? (
                <div>
                  <button
                    className={styles.deleteButton}
                    onClick={handleDelete}
                  >
                    Delete
                  </button>
                </div>
              ) : null}
            </div>
          </form>
        </div>
      </div>
    </>,
    document.getElementById("portal")!
  );
};

export default NewPostModal;
