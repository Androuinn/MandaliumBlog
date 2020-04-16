import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { BlogEntry } from '../_models/blogEntry';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Comment } from '../_models/Comment';
import { Topic } from '../_models/Topic';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  baseUrl = environment.apiUrl;
  //#region behaviour subjects, pass data via service
  blogEntryForCreationOrUpdate: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  currentBlogEntry = this.blogEntryForCreationOrUpdate.asObservable();
  blogIsWriterEntry: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  currentBlogIsWriterEntry = this.blogIsWriterEntry.asObservable();
  blogPagination: BehaviorSubject<Pagination> = new BehaviorSubject<Pagination>({
    currentPage: 1, itemsPerPage: 7, totalItems: 1, totalPages: 1});
  currentPagination = this.blogPagination.asObservable();
  // TODO most read entries i düzelt
  // blogMostReadEntries: BehaviorSubject<BlogEntry[]> = new BehaviorSubject<BlogEntry[]>(null);
  // currentBlogMostReadEntries = this.blogMostReadEntries.asObservable();
   //#endregion
  constructor(private http: HttpClient) {}

  changeBlogEntryForCreationOrUpdate(entry: number) {
    this.blogEntryForCreationOrUpdate.next(entry);
  }

  changeBlogIsWriterEntry(writerEntry: boolean) {
    this.blogIsWriterEntry.next(writerEntry);
  }

  changeBlogPagination(pagination: Pagination) {
    this.blogPagination.next(pagination);
  }




  //#region get methods
  getBlogEntries(page?, itemsPerPage?, userParams?, writerId?): Observable<PaginatedResult<BlogEntry[]>> {
    const paginatedResult: PaginatedResult<BlogEntry []> = new PaginatedResult<BlogEntry []>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('writerEntry', userParams);
    }

    if (writerId >= 1) {
      params = params.append('writerId', writerId);
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

  getTopics(): Observable<Topic[]> {
    return this.http.get<Topic[]>(this.baseUrl + 'blogentry/gettopics');
  }


  //#endregion

  //#region save methods
  saveBlogEntry(blogEntry: BlogEntry) {
    return this.http.post(this.baseUrl + 'blogentry', blogEntry);
  }

  updateBlogEntry(blogentry: BlogEntry) {
    return this.http.put(this.baseUrl + 'blogentry/UpdateBlogEntry', blogentry);
  }

  deleteBlogEntry(id: number) {
    return this.http.put(this.baseUrl + 'blogentry/deleteblogentry', id);
  }

  saveComment(comment: Comment) {
    return this.http.post(this.baseUrl + 'blogentry/writecomment', comment);
  }

  saveTopic(topic: Topic) {
    return this.http.post(this.baseUrl + 'blogentry/savetopic', topic);
  }
  //#endregion

}
