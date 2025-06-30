import axios from "axios";

import Picture from "../types/Picture";
import api from "./api";

export interface CreatePictureRequest {
  year: number;
  name: string;
  monthWatched: number;
  yearReleased: number;
  cinemaId: string;
  zinger: string | null;
  alias: string | null;
  isFavorite: boolean;
  isKino: boolean;
  isNewRelease: boolean;
}

export interface UpdatePictureRequest {
  id: string;
  name: string;
  year: number;
  monthWatched: number;
  yearReleased: number;
  cinemaId: string;
  zinger: string | null;
  alias: string | null;
  isFavorite: boolean;
  isKino: boolean;
  isNewRelease: boolean;
  imageUrl: string | null;
  imageObjectKey: string | null;
  altImageUrl: string | null;
  altImageObjectKey: string | null;
}

export const createPicture = async (
  data: CreatePictureRequest
): Promise<Picture> => {
  const res = await api.post<Picture>(`/pictures/${data.year}`, {
    name: data.name,
    monthWatched: data.monthWatched,
    yearReleased: data.yearReleased,
    cinemaId: data.cinemaId,
    zinger: data.zinger,
    alias: data.alias,
    isFavorite: data.isFavorite,
    isKino: data.isKino,
    isNewRelease: data.isNewRelease,
  });

  return res.data;
};

export const updatePicture = async (
  data: UpdatePictureRequest
): Promise<Picture> => {
  const res = await api.put<Picture>(`/pictures/${data.id}`, {
    name: data.name,
    yearWatched: data.year,
    monthWatched: data.monthWatched,
    yearReleased: data.yearReleased,
    cinemaId: data.cinemaId,
    zinger: data.zinger,
    alias: data.alias,
    isFavorite: data.isFavorite,
    isKino: data.isKino,
    isNewRelease: data.isNewRelease,
    imageUrl: data.imageUrl,
    imageObjectKey: data.imageObjectKey,
    altImageUrl: data.altImageUrl,
    altImageObjectKey: data.altImageObjectKey,
  });

  return res.data;
};

export const deletePicture = async (id: string): Promise<void> => {
  await api.delete(`/pictures/${id}`);
};

export const getPresignedImageUrl = async ({
  id,
  extension,
  isAlt,
}: {
  id: string;
  extension: string;
  isAlt: boolean;
}): Promise<{ presignedUploadUrl: string }> => {
  const res = await api.post(`/pictures/${id}/image-url`, {
    fileExtension: extension,
    isAlt: isAlt,
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
