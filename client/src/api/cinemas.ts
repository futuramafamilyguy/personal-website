import Cinema from "../types/Cinema";
import api from "./api";

export interface CreateCinemaRequest {
  name: string;
  city: string;
}

export const createCinema = async (
  data: CreateCinemaRequest
): Promise<Cinema> => {
  const res = await api.post<Cinema>(`/cinemas`, {
    name: data.name,
    city: data.city,
  });

  return res.data;
};
