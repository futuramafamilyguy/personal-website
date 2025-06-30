import axios from "axios";
import { FormEvent, useEffect, useState } from "react";
import ReactDom from "react-dom";

import {
  createPost,
  deletePost,
  getPresignedImageUrl,
  getPresignedMarkdownUrl,
  updatePost,
  UpdatePostRequest,
  uploadImageToPresignedUrl,
  uploadMarkdownToPresignedUrl,
} from "../../../api/posts";
import Post from "../../../types/Post";
import styles from "./NewPostModal.module.css";

interface NewPostModalProps {
  isOpen: boolean;
  onClose: () => void;
  post: Post | null;
  setTrigger: () => void;
}

const NewPostModal: React.FC<NewPostModalProps> = ({
  isOpen,
  onClose,
  post,
  setTrigger,
}) => {
  const [title, setTitle] = useState("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState<string | null>(null);
  const [imageObjectKey, setImageObjectKey] = useState("");
  const [markdownContent, setMarkdownContent] = useState("");
  const [markdownUrl, setMarkdownUrl] = useState<string | null>(null);
  const [markdownObjectKey, setMarkdownObjectKey] = useState("");
  const [createdAtUtc, setCreatedAtUtc] = useState<Date | undefined>(undefined);

  const [result, setResult] = useState("");

  useEffect(() => {
    setResult("");
    setTitle(post ? post.title : "");
    setImageFile(null);
    setImageUrl(post && post.imageUrl ? post.imageUrl : "");
    setImageObjectKey(post && post.imageObjectKey ? post.imageObjectKey : "");
    setMarkdownUrl(post && post.markdownUrl ? post.markdownUrl : "");
    setCreatedAtUtc(post ? post.createdAtUtc : undefined);
    setMarkdownObjectKey(
      post && post.markdownObjectKey ? post.markdownObjectKey : ""
    );
  }, [isOpen]);

  useEffect(() => {
    const fetchMarkdown = async () => {
      try {
        const response = await axios.get(post!.markdownUrl);
        setMarkdownContent(response.data);
      } catch (error) {
        console.error("Error fetching markdown content:", error);
      }
    };

    setMarkdownContent("");
    if (post && post.markdownUrl) {
      fetchMarkdown();
    }
  }, [isOpen]);

  const orchestrateCreatePost = async () => {
    try {
      if (!markdownContent || markdownContent.trim() === "") {
        throw new Error("post content is empty");
      }

      const newPost = await createPost(title);

      await orchestrateUploadMarkdown(newPost.id, markdownContent);

      if (imageFile) {
        await orchestrateUploadImage(newPost.id);
      }

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error creating post:", error);
      setResult("error creating post");
    }
  };

  const orchestrateUpdatePost = async () => {
    try {
      if (!markdownContent || markdownContent.trim() === "") {
        throw new Error("post content is empty");
      }

      const data: UpdatePostRequest = {
        id: post!.id,
        title: title,
        createdAtUtc: createdAtUtc!,
        markdownUrl: markdownUrl,
        markdownObjectKey: markdownObjectKey,
        imageUrl: imageObjectKey ? imageUrl : null, // use object key to determine image presence due to default image
        imageObjectKey: imageObjectKey ? imageObjectKey : null,
      };
      const updatedPost = await updatePost(data);

      await orchestrateUploadMarkdown(updatedPost.id, markdownContent);

      if (imageFile) {
        await orchestrateUploadImage(updatedPost.id);
      }

      setTrigger();
      onClose();
    } catch (error) {
      console.error("error updating post:", error);
      setResult("error updating post");
    }
  };

  const orchestrateUploadMarkdown = async (
    id: string,
    markdownContent: string
  ) => {
    const blob = new Blob([markdownContent], { type: "text/markdown" });
    const { presignedUploadUrl } = await getPresignedMarkdownUrl(id);
    await uploadMarkdownToPresignedUrl(presignedUploadUrl, blob);
  };

  const orchestrateUploadImage = async (id: string) => {
    const extension = imageFile!.name.split(".").pop()?.toLowerCase();
    const { presignedUploadUrl } = await getPresignedImageUrl(id, extension!);

    await uploadImageToPresignedUrl(presignedUploadUrl, imageFile!);
  };

  const orchestrateDeletePost = async () => {
    try {
      await deletePost(post!.id);

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error deleting post:", error);
      setResult("error deleting post");
    }
  };

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    if (post) {
      await orchestrateUpdatePost();
    } else {
      await orchestrateCreatePost();
    }
  };

  const handleDelete = async (event: FormEvent) => {
    event.preventDefault();
    await orchestrateDeletePost();
  };

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
              <label>title</label>
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
              <label>image</label>
              <input
                type="file"
                onChange={(e) =>
                  setImageFile(e.target.files ? e.target.files[0] : null)
                }
              />
            </div>
            <div className={styles.formGroup}>
              <label>take</label>
              <textarea
                value={markdownContent}
                onChange={(e) => setMarkdownContent(e.target.value)}
                placeholder="drop your take here"
              />
            </div>
            <div className={styles.buttonContainer}>
              <div>
                <button className={styles.button} type="submit">
                  submit
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
                    delete
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
