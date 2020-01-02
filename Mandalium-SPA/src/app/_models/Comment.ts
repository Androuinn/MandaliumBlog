export interface Comment {
  id: string;
  commenterName: string;
  email: string;
  commentString: string;
  createdDate: Date;
  blogEntryId: number;
}
