import { Component, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Writer } from '../_models/Writer';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  writers: Writer[];
  openFullBackground = false;
  constructor(private blogService: BlogService) { }

  ngOnInit() {
    this.getWriters();
  }

  getWriters() {
    return this.blogService.getWriters().subscribe((res: Writer[]) => {
      this.writers = res;
    });
  }



}
