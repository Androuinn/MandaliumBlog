import { Comment } from './Comment';

export interface BlogEntry {
    id: number;
    headline: string;
    subHeadline: string;
    innerTextHtml: string;
    createdDate: Date;
    writerName: string;
    writerSurname: string;
    topicName: number;
    photoUrl: string;
    comments: Comment[];
}
