import { Component, OnInit } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute } from '@angular/router';
import { Title, Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-blog-writerentry',
  templateUrl: './blog-writerEntry.component.html',
  styleUrls: ['./blog-writerEntry.component.css']
})
export class BlogWriterEntryComponent implements OnInit {
  blogEntries: BlogEntry[];
  pagination: Pagination;
  constructor(private blogService: BlogService, private route: ActivatedRoute, private titleService: Title, private metaTagService: Meta) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      const x = data.entries as PaginatedResult<BlogEntry[]>;
      this.blogEntries = x.result;
      this.pagination = x.pagination;
    });

    this.titleService.setTitle('Kendime Düşünceler');
    this.metaTagService.updateTag({name: 'description', content: 'Kendime Düşünceler'});
    this.metaTagService.updateTag({property: 'og:site_name', content: 'Mandalium'});
    this.metaTagService.updateTag({property: 'og:url', content: 'https://mandalium.azurewebsites.net/personalblog'});
    this.metaTagService.updateTag({property: 'og:title', content: 'Mandalium | Kendime Düşünceler'});
    this.metaTagService.updateTag({property: 'og:description', content: 'Kendime Düşünceler'});
  }

  loadBlogEntries() {
    this.blogService
      .getBlogEntries(this.pagination.currentPage, this.pagination.itemsPerPage, true)
      .subscribe(
        (res: PaginatedResult<BlogEntry[]>) => {
          this.blogEntries = res.result;
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
