import { Component, OnInit } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute } from '@angular/router';
import { Comment } from 'src/app/_models/Comment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Pagination } from 'src/app/_models/pagination';
import { Title, Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-blog-detailed',
  templateUrl: './blog-detailed.component.html',
  styleUrls: ['./blog-detailed.component.css']
})
export class BlogDetailedComponent implements OnInit {
  blogEntry: BlogEntry;
  comment: Comment;
  commentFormGroup: FormGroup;
  pagination: Pagination;
  constructor(
    private blogService: BlogService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private titleService: Title,
    private metaTagService: Meta
  ) {
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.blogEntry = data.blog;
      this.pagination = this.blogEntry.comments.pagination;
      this.commentFormGroup = this.fb.group({
        commenterName: ['', [Validators.required, Validators.maxLength(100)]],
        email: ['',  [Validators.required, Validators.email, Validators.maxLength(100)]],
        commentString: ['', [Validators.required, Validators.maxLength(500)]],
        blogEntryId: data.blog.id,
        writerId: Number
      });
    });

    this.titleService.setTitle(this.blogEntry.headline.toString());
    this.metaTagService.updateTag({name: 'description', content: this.blogEntry.headline.toString()});
    this.metaTagService.updateTag({property: 'og:type', content: 'article'});
    this.metaTagService.updateTag({property: 'og:url', content: 'https://mandalium.azurewebsites.net/blog/' + this.blogEntry.id + '/' + this.blogEntry.headline});
    this.metaTagService.updateTag({property: 'og:image', content: this.blogEntry.photoUrl.toString()});
    this.metaTagService.updateTag({property: 'og:title', content: this.blogEntry.headline});
    this.metaTagService.updateTag({property: 'og:description', content: this.blogEntry.subHeadline.toString()});
  }

  writeComment() {
    if (this.commentFormGroup.valid) {
      this.blogService.saveComment(this.commentFormGroup.value).subscribe(() => {
          console.log('writing comment successful');
          this.loadBlogEntry();
          this.commentFormGroup.reset({
            blogEntryId: this.blogEntry.id
          });
        }, error => {
          console.error(error);
        });
    }
  }

  loadBlogEntry() {
    this.blogService.getBlogEntry(this.route.snapshot.params.id, this.pagination.currentPage, this.pagination.itemsPerPage, true).subscribe(
      (blogEntry: BlogEntry) => {
        this.blogEntry.comments = blogEntry.comments;
        this.pagination = blogEntry.comments.pagination;
      },
      error => {
        console.error(error);
      }
    );
  }


  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadBlogEntry();
  }
}
