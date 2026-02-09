export default interface Post {
  id: string;
  title: string;
  createdAtUtc: Date;
  lastUpdatedUtc: Date;
  imageUrl: string;
  imageObjectKey: string;
  markdownUrl: string;
  markdownObjectKey: string;
  slug: string;
  markdownVersion: number;
  isPublished: boolean;
}
