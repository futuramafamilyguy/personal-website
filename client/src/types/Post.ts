export default interface Post {
  id: string;
  title: string;
  createdAtUtc: Date;
  lastUpdatedUtc: Date;
  imageUrl: string;
  contentUrl: string;
}
