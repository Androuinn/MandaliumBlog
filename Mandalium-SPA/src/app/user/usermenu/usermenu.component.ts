import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { BlogService } from 'src/app/_services/blog.service';
import { Topic } from 'src/app/_models/Topic';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { PhotoService } from 'src/app/_services/photo.service';
import { Photo } from 'src/app/_models/Photo';

@Component({
  selector: 'app-usermenu',
  templateUrl: './usermenu.component.html',
  styleUrls: ['./usermenu.component.css'],
})
export class UsermenuComponent implements OnInit {

  blogEntries: BlogEntry[];
  pagination: Pagination;
  postsOpen = false;


  constructor(
    private router: Router,
    public authService: AuthService,
    private blogService: BlogService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 15,
      totalPages: 1,
      totalItems: 1,
    };

  }

  openPosts() {
    this.loadBlogEntries();
    this.postsOpen = !this.postsOpen;
  }


  loadBlogEntries() {
    this.blogService
      .getBlogEntries(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        null,
        this.authService.decodedToken.nameid
      )
      .subscribe(
        (res: PaginatedResult<BlogEntry[]>) => {
          this.blogEntries = res.result;
          this.pagination = res.pagination;
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

  // post numarasını sıfıra çekiyor eğer değiştirme yaptıysa 1 de kalıyor
  changeEntry(entry: number) {
    this.blogService.changeBlogEntry(entry);
    this.router.navigate(['/create']);
  }




}
