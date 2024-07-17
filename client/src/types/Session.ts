export interface Session {
  movie: SessionPicture;
  cinemas: Cinema[];
  showtimes: Date[];
}

export interface Cinema {
  name: string;
  homePageUrl: string;
}

interface SessionPicture {
  name: string;
  yearReleased: number;
  imageUrl: string;
}
