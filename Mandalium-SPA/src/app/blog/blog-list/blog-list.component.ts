import {
  Component,
  OnInit,
} from '@angular/core';
import { BlogService } from 'src/app/_services/blog.service';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
import { MetaService } from 'src/app/_services/meta.service';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css'],
})
export class BlogListComponent implements OnInit {
  blogEntries: BlogEntry[];
  pagination: Pagination;
  fragment: any;
  baseUrl = environment.apiUrl + 'blogentry';
  constructor(
    private blogService: BlogService,
    private route: ActivatedRoute,
    private metaService: MetaService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      const x = data.entries as PaginatedResult<BlogEntry[]>;
      this.blogEntries = x.result;
      this.blogEntries.forEach(element => {
        if (element.photoUrl == null || element.photoUrl === '') {
          element.photoUrl = '../../assets/çzgisiz logo.png';
        }
      });
      this.pagination = x.pagination;
    });

    this.metaService.UpdateTags('Mandalium | En Son Haberler', 'En Son Haberler',
    'Blog', 'Mandalium | En Son Haberler', 'En Son Haberler', null);
  }

  loadBlogEntries() {
    this.blogService
      .getBlogEntries(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        false
      )
      .subscribe(
        (res: PaginatedResult<BlogEntry[]>) => {
          this.blogEntries = res.result;
          this.pagination = res.pagination;
          this.blogService.changeBlogPagination(this.pagination);
        },
        (error) => {
          console.error(error);
        }
      );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadBlogEntries();
  }
}
