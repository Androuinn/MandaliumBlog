import { Component, OnInit } from '@angular/core';
import { BlogService } from '../_services/blog.service';
import { Writer } from '../_models/Writer';
import { UserService } from '../_services/user.service';
import { Title, Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
})
export class AboutComponent implements OnInit {
  writers: Writer[];
  openFullBackground = false;
  constructor(
    private userService: UserService,
    private titleService: Title,
    private metaTagService: Meta
  ) {}

  ngOnInit() {
    this.getWriters();

    this.titleService.setTitle('Hakkımda');
    this.metaTagService.updateTag({name: 'description', content: 'Hakkımda'});
    this.metaTagService.updateTag({property: 'og:url', content: 'https://mandalium.azurewebsites.net/about'});
    this.metaTagService.updateTag(
      {property: 'og:image', content: 'https://res.cloudinary.com/dpwbfco4g/image/upload/v1587061001/%C3%A7zgisiz_logo_ddiqau.png'});
    this.metaTagService.updateTag({property: 'og:title', content: 'Hakkımda'});
    this.metaTagService.updateTag({property: 'og:description', content: 'Hakkımda'});

  }

  getWriters() {
    return this.userService.getWriters().subscribe((res: Writer[]) => {
      this.writers = res;
    });
  }
}
