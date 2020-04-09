import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BlogService } from 'src/app/_services/blog.service';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css']
})
export class BlogListComponent implements OnInit {
  blogEntries: BlogEntry[];
  pagination: Pagination;
  constructor(private blogService: BlogService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 7,
      totalPages: 1,
      totalItems: 1
    };
    this.loadBlogEntries();
  }

  loadBlogEntries() {
    this.blogService
      .getBlogEntries(this.pagination.currentPage, this.pagination.itemsPerPage, false)
      .subscribe(
        (res: PaginatedResult<BlogEntry[]>) => {
          this.blogEntries = res.result;
          this.pagination = res.pagination;
        },
        error => {
          console.error(error);
        }
      );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadBlogEntries();
  }


}
