import Cinema from "./Cinema";

export default interface Picture {
  id: string;
  name: string;
  imageUrl: string;
  alias: string;
  yearReleased: number;
  zinger: string;
  cinema: Cinema;
  monthWatched: number;
  isFavorite: boolean;
  isKino: boolean;
  isNewRelease: boolean;
  yearWatched: number;
  altImageUrl: string;
}
