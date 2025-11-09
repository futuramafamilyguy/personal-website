import Cinema from "./Cinema";

export default interface Movie {
  id: string;
  name: string;
  year: number;
  month: number;
  cinema: Cinema;
  releaseYear: number;
  zinger: string;
  isNominated: boolean;
  isKino: boolean;
  isRetro: boolean;
  alias: string;
  motif: string;
  imageUrl: string;
  imageObjectKey: string;
  altImageUrl: string;
  altImageObjectKey: string;
}
