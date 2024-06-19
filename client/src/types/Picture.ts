interface Cinema {
  name: string;
  city: string;
}

export interface Picture {
  id: string;
  name: string;
  imageUrl: string;
  alias: string;
  year: number;
  zinger: string;
  cinema: Cinema
}
