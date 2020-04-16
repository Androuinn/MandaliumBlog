import { Component, OnInit } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-blog-writerentry',
  templateUrl: './blog-writerEntry.component.html',
  styleUrls: ['./blog-writerEntry.component.css']
})
export class BlogWriterEntryComponent implements OnInit {
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
      .getBlogEntries(this.pagination.currentPage, this.pagination.itemsPerPage, true)
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
