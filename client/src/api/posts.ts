import axios from "axios";

import Post from "../types/Post";
import api from "./api";

export interface UpdatePostRequest {
  id: string;
  title: string;
  createdAtUtc: Date;
  markdownUrl: string | null;
  markdownObjectKey: string | null;
  imageUrl: string | null;
  imageObjectKey: string | null;
  markdownVersion: number;
}

export const createPost = async (title: string): Promise<Post> => {
  const res = await api.post<Post>(`/posts`, {
    title: title,
  });

  return res.data;
};

export const updatePost = async (data: UpdatePostRequest): Promise<Post> => {
  const res = await api.put<Post>(`/posts/${data.id}`, {
    title: data.title,
    createdAtUtc: data.createdAtUtc,
    markdownUrl: data.markdownUrl,
    markdownObjectKey: data.markdownObjectKey,
    imageUrl: data.imageUrl,
    imageObjectKey: data.imageObjectKey,
    markdownVersion: data.markdownVersion,
  });

  return res.data;
};

export const deletePost = async (id: string): Promise<void> => {
  await api.delete(`/posts/${id}`);
};

export const getPresignedMarkdownUrl = async (
  id: string
): Promise<{ presignedUploadUrl: string }> => {
  const res = await api.post(`/posts/${id}/markdown-url`);

  return res.data;
};

export const uploadMarkdownToPresignedUrl = async (
  presignedUrl: string,
  data: Blob
) => {
  return axios.put(presignedUrl, data, {
    headers: {
      "Content-Type": "text/markdown",
    },
  });
};

export const getPresignedImageUrl = async (
  id: string,
  extension: string
): Promise<{ presignedUploadUrl: string }> => {
  const res = await api.post(`/posts/${id}/image-url`, {
    fileExtension: extension,
  });

  return res.data;
};

export const uploadImageToPresignedUrl = async (
  presignedUrl: string,
  file: File
) => {
  return axios.put(presignedUrl, file, {
    headers: {
      "Content-Type": file.type,
    },
  });
};
