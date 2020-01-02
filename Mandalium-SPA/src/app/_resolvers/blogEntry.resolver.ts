import { Injectable } from '@angular/core';
import { BlogEntry } from '../_models/blogEntry';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { BlogService } from '../_services/blog.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class BlogEntryResolver implements Resolve<BlogEntry> {
    constructor(private blogService: BlogService, private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): Observable<BlogEntry> {
        return this.blogService.getBlogEntry(route.params.id).pipe(
            catchError(error => {
                console.error('Veri alma hatası');
                this.router.navigate(['/']);
                return of(null);
            })
        );
    }
}
