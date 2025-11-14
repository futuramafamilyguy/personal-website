export interface Track {
  title: string;
  artist: string;
  url: string;
  original: string;
}

export interface Segment {
  cover: string;
  intro: string;
  playlist: Track[];
}
