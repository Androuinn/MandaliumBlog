import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BlogEntry } from '../_models/blogEntry';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { WriterTopic } from '../_models/WriterTopic';
import { Comment } from '../_models/Comment';
import { Topic } from '../_models/Topic';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getBlogEntries(page?, itemsPerPage?, userParams?): Observable<PaginatedResult<BlogEntry[]>> {
    const paginatedResult: PaginatedResult<BlogEntry []> = new PaginatedResult<BlogEntry []>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('writerEntry', userParams);
    }


    return this.http.get<BlogEntry[]>(this.baseUrl + 'blogentry', {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getBlogEntry(id, commentPage?, commentsPerPage?, userParams?): Observable<BlogEntry> {
    let blogEntry: BlogEntry;

    let params = new HttpParams();

    if (commentPage != null && commentsPerPage != null) {
      params = params.append('pageNumber', commentPage);
      params = params.append('pageSize', commentsPerPage);
    }
    if (userParams != null) {
      params = params.append('EntryAlreadyPicked', userParams);
    }

    return this.http.get<BlogEntry>(this.baseUrl + 'blogentry/' + id, {observe: 'response', params}).pipe(
      map(response => {
        blogEntry = response.body;
        if (response.headers.get('Pagination') != null) {
          response.body.comments.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return blogEntry;
      })
    );
  }

  getMostRead(): Observable<BlogEntry[]> {
   return this.http.get<BlogEntry[]>(this.baseUrl + 'blogentry/getmostread');
  }

  getTopicsAndWriters(): Observable<WriterTopic> {
    return this.http.get<WriterTopic>(this.baseUrl + 'blogentry/gettopicandwriter');
  }

  saveBlogEntry(blogEntry: BlogEntry) {
    return this.http.post(this.baseUrl + 'blogentry', blogEntry);
  }
  saveComment(comment: Comment) {
    return this.http.post(this.baseUrl + 'blogentry/writecomment', comment);
  }

  saveTopic(Topic: Topic) {
    return this.http.post(this.baseUrl + 'blogentry/savetopic', Topic);
  }

}
