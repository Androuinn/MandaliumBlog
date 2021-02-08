import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { PaginatedResult } from "src/app/_models/pagination";
import { BlogService } from "src/app/_services/blog.service";
import { Comment } from "src/app/_models/Comment";
import { AuthService } from "src/app/_services/auth.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AlertifyService } from "src/app/_services/alertify.service";
import { environment } from "src/environments/environment";

@Component({
  selector: "app-blog-comments",
  templateUrl: "./blog-comments.component.html",
  styleUrls: ["./blog-comments.component.css"],
})
export class BlogCommentsComponent implements OnInit {
  commentList: PaginatedResult<Comment[]>;
  commentFormGroup: FormGroup;
  defaultPhotoUrl = environment.defaultPhotoUrl;

  constructor(
    private blogService: BlogService,
    private route: ActivatedRoute,
    public authService: AuthService,
    private fb: FormBuilder,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.commentList = new PaginatedResult<Comment[]>();
      this.loadComments();
      if (this.authService.loggedIn()) {
        this.commentFormGroup = this.fb.group({
          commentString: ["", [Validators.required, Validators.maxLength(500)]],
          blogEntryId: data.blog.id,
          userId: this.authService.decodedToken.nameid,
        });
      } else {
        this.commentFormGroup = this.fb.group({
          commenterName: ["", [Validators.required, Validators.maxLength(100)]],
          email: [ "", [Validators.required, Validators.email, Validators.maxLength(100)]],
          commentString: ["", [Validators.required, Validators.maxLength(500)]],
          blogEntryId: data.blog.id,
          userId: Number,
        });
      }
    });
  }

  loadComments() {
    this.blogService
      .getComments(this.route.snapshot.params.id, this.commentList.pagination.currentPage, this.commentList.pagination.itemsPerPage)
      .subscribe(
        (comments: PaginatedResult<Comment[]>) => {
          this.commentList.result = comments.result;
          this.commentList.pagination = comments.pagination;
        },
        (error) => {
          console.error(error);
        }
      );
  }

  writeComment() {
    if (this.commentFormGroup.valid) {
      this.blogService.saveComment(this.commentFormGroup.value).subscribe(
        (res: Comment) => {
          this.alertify.success("Yorum Yazma Başarılı");

          res.commenterName = res.commenterName == null ? this.authService.decodedToken?.unique_name : res.commenterName;
          res.photoUrl = this.defaultPhotoUrl;

          this.commentList.result.pop();
          this.commentList.result.unshift(res);
          this.commentList.pagination.totalItems++;

          if (this.authService.loggedIn()) {
            this.commentFormGroup.reset({
              blogEntryId: this.route.snapshot.params.id,
              userId: this.authService.decodedToken.nameid,
            });
          } else {
            this.commentFormGroup.reset({
              blogEntryId: this.route.snapshot.params.id,
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

  pageChanged(event: any): void {
    this.commentList.pagination.currentPage = event.page;
    this.loadComments();
  }

  isArray(obj : any ) {
    return Array.isArray(obj)
 }
}
