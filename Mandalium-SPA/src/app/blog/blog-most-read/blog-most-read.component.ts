import { Component, OnInit, Input } from '@angular/core';
import { BlogEntry } from 'src/app/_models/blogEntry';
import { BlogService } from 'src/app/_services/blog.service';
import { environment } from 'src/environments/environment';



@Component({
  selector: 'app-blog-most-read',
  templateUrl: './blog-most-read.component.html',
  styleUrls: ['./blog-most-read.component.css']
})
export class BlogMostReadComponent implements OnInit {
  @Input()  mostReadEntries: any;
  mostReadBlogEntries: BlogEntry[];
  constructor(private blogService: BlogService) { }

  ngOnInit() {
   this.getMostRead();
  }


  getMostRead() {
    this.blogService.getMostRead().subscribe((mostReadEntries: any) => {
      this.mostReadBlogEntries = mostReadEntries;
      this.mostReadBlogEntries.forEach(element => {
        if (element.photoUrl == null || element.photoUrl === '') {
          element.photoUrl = environment.defaultPhotoUrl;
        }
      });
    }, error => {
      console.error(error);
    });
  }




}
