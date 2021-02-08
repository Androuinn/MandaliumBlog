import { Component, OnInit } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from 'src/app/_models/pagination';
import { AuthService } from 'src/app/_services/auth.service';
import { MetaService } from 'src/app/_services/meta.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-blog-detailed',
  templateUrl: './blog-detailed.component.html',
  styleUrls: ['./blog-detailed.component.css'],
})
export class BlogDetailedComponent implements OnInit {
  blogEntry: BlogEntry;
  pagination: Pagination;

  constructor(
    private blogService: BlogService,
    private route: ActivatedRoute,
    public authService: AuthService,
    private metaService: MetaService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.blogEntry = data.blog;
      if (this.blogEntry.photoUrl == null || this.blogEntry.photoUrl === '') {
        this.blogEntry.photoUrl =  environment.defaultPhotoUrl;
      }
    } );

    this.metaService.UpdateTags(this.blogEntry.headline.toString(), this.blogEntry.headline.toString(), 'blog/' +
    this.blogEntry.id.toString() + '/' + this.blogEntry.headline.toString(),
    this.blogEntry.headline.toString(), this.blogEntry.subHeadline.toString(), this.blogEntry.photoUrl.toString() );
  }
}
