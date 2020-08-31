import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { User } from '../_models/Writer';
import { UserService } from '../_services/user.service';
import { Title, Meta } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
})
export class AboutComponent implements OnInit, AfterViewChecked {
  writers: User[];
  writer: User;
  openFullBackground = false;
  fragment: any;
  constructor(
    private userService: UserService,
    private titleService: Title,
    private metaTagService: Meta,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.getWriters();

    this.titleService.setTitle('Hakkımda');
    this.metaTagService.updateTag({ name: 'description', content: 'Hakkımda' });
    this.metaTagService.updateTag({
      property: 'og:url',
      content: 'https://mandalium.azurewebsites.net/about',
    });
    this.metaTagService.updateTag({
      property: 'og:image',
      content:
        'https://res.cloudinary.com/dpwbfco4g/image/upload/v1587061001/%C3%A7zgisiz_logo_ddiqau.png',
    });
    this.metaTagService.updateTag({
      property: 'og:title',
      content: 'Hakkımda',
    });
    this.metaTagService.updateTag({
      property: 'og:description',
      content: 'Hakkımda',
    });
  }

  ngAfterViewChecked(): void {
    try {
      if (this.fragment) {
        document.querySelector('#' + this.fragment).scrollIntoView();
      }
    } catch (e) {}
  }
  getWriters() {
    return this.userService.getUsers().subscribe((res: User[]) => {
      this.writers = res;
      this.writers.forEach((element) => {
        if (element.photoUrl == null || element.photoUrl === '') {
          element.photoUrl = '../../assets/çzgisiz logo.png';
        }
        element.collapse = false;
      });
      this.route.fragment.subscribe((fragment) => {
        this.fragment = fragment;
      });
    });
  }

  getWriter() {
    return this.userService.getUser().subscribe((res: User) => {
      this.writer = res;
      if (this.writer.photoUrl == null || this.writer.photoUrl === '') {
        this.writer.photoUrl = '../../assets/çzgisiz logo.png';
      }
      this.writer.collapse = false;
    });
  }
}
