export interface Session {
  title: string;
  releaseYear: number;
  imageUrl: string;
  cinemas: Cinema[];
  showtimes: Date[];
}

export interface Cinema {
  name: string;
  homepageUrl: string;
}
