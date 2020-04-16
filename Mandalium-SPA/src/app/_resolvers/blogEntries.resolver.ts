import { Injectable } from '@angular/core';
import { BlogEntry } from '../_models/blogEntry';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { BlogService } from '../_services/blog.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Pagination } from '../_models/pagination';

@Injectable()
export class BlogEntriesResolver implements Resolve<BlogEntry[]> {
    pagination: Pagination;
    writerEntry: boolean;
    constructor(private blogService: BlogService, private router: Router) {
        this.blogService.currentPagination.subscribe(res => this.pagination = res);
        this.blogService.currentBlogIsWriterEntry.subscribe(res => this.writerEntry = res);
    }

    resolve(route: ActivatedRouteSnapshot): Observable<BlogEntry[]> {
        return this.blogService.getBlogEntries(this.pagination.currentPage, this.pagination.itemsPerPage, this.writerEntry ).pipe(
            catchError(error => {
                console.error('Veri alma hatası');
                this.router.navigate(['/']);
                return of(null);
            })
        );
    }

}
