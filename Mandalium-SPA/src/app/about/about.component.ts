import { Component, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Writer } from '../_models/Writer';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  writers: Writer[];
  openFullBackground = false;
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getWriters();
  }

  getWriters() {
    return this.userService.getWriters().subscribe((res: Writer[]) => {
      this.writers = res;
    });
  }



}
