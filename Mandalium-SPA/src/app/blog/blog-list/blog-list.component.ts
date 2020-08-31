import {
  Component,
  OnInit,
} from '@angular/core';
import { BlogService } from 'src/app/_services/blog.service';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { ActivatedRoute } from '@angular/router';
import { Title, Meta } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

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
    private titleService: Title,
    private metaTagService: Meta,
    private http: HttpClient
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

    this.titleService.setTitle('Mandalium | En Son Haberler');
    this.metaTagService.updateTag({
      name: 'description',
      content: 'En Son Haberler',
    });
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
