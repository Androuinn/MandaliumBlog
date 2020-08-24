import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute } from '@angular/router';
import { Comment } from 'src/app/_models/Comment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { Title, Meta } from '@angular/platform-browser';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-blog-detailed',
  templateUrl: './blog-detailed.component.html',
  styleUrls: ['./blog-detailed.component.css'],
})
export class BlogDetailedComponent implements OnInit {
  blogEntry: BlogEntry;
  commentFormGroup: FormGroup;
  pagination: Pagination;
  constructor(
    private blogService: BlogService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private titleService: Title,
    private metaTagService: Meta,
    private alertify: AlertifyService,
    public authService: AuthService,
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.blogEntry = data.blog;
      this.pagination = this.blogEntry.comments.pagination;
      if (this.authService.loggedIn()) {
        this.commentFormGroup = this.fb.group({
          commentString: ['', [Validators.required, Validators.maxLength(500)]],
          blogEntryId: data.blog.id,
          userId: this.authService.decodedToken.nameid,
        });
      } else {
        this.commentFormGroup = this.fb.group({
          commenterName: ['', [Validators.required, Validators.maxLength(100)]],
          email: [ '', [Validators.required, Validators.email, Validators.maxLength(100)] ],
          commentString: ['', [Validators.required, Validators.maxLength(500)]],
          blogEntryId: data.blog.id,
          userId: Number,
        });
      }
    });

    this.titleService.setTitle(this.blogEntry.headline.toString());
    this.metaTagService.updateTag({
      name: 'description',
      content: this.blogEntry.headline.toString(),
    });
    this.metaTagService.updateTag({ property: 'og:type', content: 'article' });
    this.metaTagService.updateTag({
      property: 'og:url',
      content:
        'https://mandalium.azurewebsites.net/blog/' +
        this.blogEntry.id +
        '/' +
        this.blogEntry.headline,
    });
    this.metaTagService.updateTag({
      property: 'og:image',
      content: this.blogEntry.photoUrl.toString(),
    });
    this.metaTagService.updateTag({
      property: 'og:title',
      content: this.blogEntry.headline,
    });
    this.metaTagService.updateTag({
      property: 'og:description',
      content: this.blogEntry.subHeadline.toString(),
    });
  }

  writeComment() {
    if (this.commentFormGroup.valid) {
      this.blogService.saveComment(this.commentFormGroup.value).subscribe(
        (res: Comment) => {
          this.alertify.success('Yorum Yazma Başarılı');
          // this.loadComments();
          this.loadBlogEntry();
          if (this.authService.loggedIn()) {
            this.commentFormGroup.reset({
              blogEntryId: this.blogEntry.id,
              userId: this.authService.decodedToken.nameid,
            });
          } else {
            this.commentFormGroup.reset({
              blogEntryId: this.blogEntry.id,
              userId: Number,
            });
          }
        },
        (error) => {
          console.error(error);
        }
      );
    }
  }

  loadComments() {
    this.blogService.getComments(this.route.snapshot.params.id, this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe(
      (comments: PaginatedResult<Comment[]>) => {
        this.blogEntry.comments.result = comments.result;
        this.pagination = comments.pagination;
        this.blogEntry.comments.result.push(null);
      },
      (error) => {
        console.error(error);
      }
    );
}

  loadBlogEntry() {
    this.blogService
      .getBlogEntry( this.route.snapshot.params.id, this.pagination.currentPage, this.pagination.itemsPerPage, true)
      .subscribe(
        (blogEntry: BlogEntry) => {
          this.blogEntry.comments = blogEntry.comments;
          this.pagination = blogEntry.comments.pagination;
        },
        (error) => {
          console.error(error);
        }
      );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadBlogEntry();
  }
}
