import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BlogEntry } from '../_models/blogEntry';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { WriterTopic } from '../_models/WriterTopic';
import { Comment } from '../_models/Comment';

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

  getBlogEntry(id): Observable<BlogEntry> {
    return this.http.get<BlogEntry>(this.baseUrl + 'blogentry/' + id);
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

}
