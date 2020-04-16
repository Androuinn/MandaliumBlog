import { Injectable } from '@angular/core';
import { BlogEntry } from '../_models/blogEntry';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { BlogService } from '../_services/blog.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Pagination } from '../_models/pagination';

@Injectable()
export class BlogEntryResolver implements Resolve<BlogEntry> {
    pagination: Pagination;
    constructor(private blogService: BlogService, private router: Router) {
        this.pagination = {
            currentPage: 1,
            itemsPerPage: 10,
            totalPages: 1,
            totalItems: 1
          };
    }

    resolve(route: ActivatedRouteSnapshot): Observable<BlogEntry> {
        return this.blogService.getBlogEntry(route.params.id, this.pagination.currentPage, this.pagination.itemsPerPage ).pipe(
            catchError(error => {
                console.error('Veri alma hatası');
                this.router.navigate(['/']);
                return of(null);
            })
        );
    }

}
