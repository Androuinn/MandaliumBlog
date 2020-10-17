export interface Comment {
  id: string;
  commenterName: string;
  email: string;
  commentString: string;
  createdDate: Date;
  photoUrl: string;
  blogEntryId: number;
}
