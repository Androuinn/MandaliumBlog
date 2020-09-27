import { Component, OnInit } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute } from '@angular/router';
import { MetaService } from 'src/app/_services/meta.service';

@Component({
  selector: 'app-blog-writerentry',
  templateUrl: './blog-writerEntry.component.html',
  styleUrls: ['./blog-writerEntry.component.css']
})
export class BlogWriterEntryComponent implements OnInit {
  blogEntries: BlogEntry[];
  pagination: Pagination;
  constructor(private blogService: BlogService, private route: ActivatedRoute, private metaService: MetaService) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      const x = data.entries as PaginatedResult<BlogEntry[]>;
      this.blogEntries = x.result;
      this.blogEntries.forEach(element => {
        if (element.photoUrl == null || element.photoUrl === '') {
          element.photoUrl = '../../assets/çzgisiz logo.png';
        }
      });
      this.pagination = x.pagination;
    });

    this.metaService.UpdateTags('Kendime Düşünceler', 'Kendime Düşünceler',
     'PersonalBlog', 'Mandalium | Kendime Düşünceler', 'Kendime Düşünceler', null);
  }

  loadBlogEntries() {
    this.blogService
      .getBlogEntries(this.pagination.currentPage, this.pagination.itemsPerPage, true)
      .subscribe(
        (res: PaginatedResult<BlogEntry[]>) => {
          this.blogEntries = res.result;
          this.blogEntries.forEach(element => {
            if (element.photoUrl == null || element.photoUrl === '') {
              element.photoUrl = '../../assets/çzgisiz logo.png';
            }
          });
          this.pagination = res.pagination;
          this.blogService.changeBlogPagination(this.pagination);
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
