import { Component, OnInit } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { BlogService } from 'src/app/_services/blog.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Comment } from 'src/app/_models/Comment';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-blog-detailed',
  templateUrl: './blog-detailed.component.html',
  styleUrls: ['./blog-detailed.component.css']
})
export class BlogDetailedComponent implements OnInit {
  blogEntry: BlogEntry;
  comment: Comment;
  commentFormGroup: FormGroup;
  constructor(
    private blogService: BlogService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.blogEntry = data.blog;
      this.commentFormGroup = this.fb.group({
        commenterName: ['', [Validators.required, Validators.maxLength(100)]],
        email: ['',  [Validators.required, Validators.email, Validators.maxLength(100)]],
        commentString: ['', [Validators.required, Validators.maxLength(500)]],
        blogEntryId: data.blog.id
      });
    });
  }

  writeComment() {
    if (this.commentFormGroup.valid) {
      this.blogService.saveComment(this.commentFormGroup.value).subscribe(() => {
          console.log('writing comment successful');
        }, error => {
          console.error(error);
        });
      this.router.navigate([ '/blog/' + this.blogEntry.id + '/' + this.blogEntry.headline]);
    }
  }

  // loadBlogEntry() {
  //   this.blogService.getBlogEntry(this.route.snapshot.params.id).subscribe(
  //     (blogEntry: BlogEntry) => {
  //       this.blogEntry = blogEntry;
  //     },
  //     error => {
  //       console.error(error);
  //     }
  //   );
  // }
}
