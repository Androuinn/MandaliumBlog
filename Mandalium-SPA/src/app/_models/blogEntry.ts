import { Comment } from './Comment';
import { PaginatedResult } from './pagination';

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
    topicId: number;
    writerEntry: boolean;
    comments: PaginatedResult<Comment[]>;
}
