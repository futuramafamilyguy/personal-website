import axios from "axios";

import Movie from "../types/Movie";
import api from "./api";

export interface CreateMovieRequest {
  year: number;
  name: string;
  month: number;
  releaseYear: number;
  cinemaId: string;
  zinger: string | null;
  alias: string | null;
  motif: string | null;
  isNominated: boolean;
  isKino: boolean;
  isRetro: boolean;
}

export interface UpdateMovieRequest {
  id: string;
  name: string;
  year: number;
  month: number;
  releaseYear: number;
  cinemaId: string;
  zinger: string | null;
  alias: string | null;
  motif: string | null;
  isNominated: boolean;
  isKino: boolean;
  isRetro: boolean;
  imageUrl: string | null;
  imageObjectKey: string | null;
  altImageUrl: string | null;
  altImageObjectKey: string | null;
}

export const createMovie = async (data: CreateMovieRequest): Promise<Movie> => {
  const res = await api.post<Movie>(`/movies/${data.year}`, {
    name: data.name,
    month: data.month,
    releaseYear: data.releaseYear,
    cinemaId: data.cinemaId,
    zinger: data.zinger,
    alias: data.alias,
    motif: data.motif,
    isNominated: data.isNominated,
    isKino: data.isKino,
    isRetro: data.isRetro,
  });

  return res.data;
};

export const updateMovie = async (data: UpdateMovieRequest): Promise<Movie> => {
  const res = await api.put<Movie>(`/movies/${data.id}`, {
    name: data.name,
    year: data.year,
    month: data.month,
    releaseYear: data.releaseYear,
    cinemaId: data.cinemaId,
    zinger: data.zinger,
    alias: data.alias,
    motif: data.motif,
    isNominated: data.isNominated,
    isKino: data.isKino,
    isRetro: data.isRetro,
    imageUrl: data.imageUrl,
    imageObjectKey: data.imageObjectKey,
    altImageUrl: data.altImageUrl,
    altImageObjectKey: data.altImageObjectKey,
  });

  return res.data;
};

export const deleteMovie = async (id: string): Promise<void> => {
  await api.delete(`/movies/${id}`);
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
  const res = await api.post(`/movies/${id}/image-url`, {
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
