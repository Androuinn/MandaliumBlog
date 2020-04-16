import { Component, OnInit } from '@angular/core';
import { Title, Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  constructor(
    private titleService: Title,
    private metaTagService: Meta
  ) { }

  ngOnInit() {

    this.titleService.setTitle('İletişim');
    this.metaTagService.updateTag({name: 'description', content: 'İletişim Adresi'});
    this.metaTagService.updateTag({property: 'og:url', content: 'https://mandalium.azurewebsites.net/contact'});
    this.metaTagService.updateTag(
      {property: 'og:image', content: 'https://res.cloudinary.com/dpwbfco4g/image/upload/v1587061001/%C3%A7zgisiz_logo_ddiqau.png'});
    this.metaTagService.updateTag({property: 'og:title', content: 'İletişim'});
    this.metaTagService.updateTag({property: 'og:description', content: 'İletişim Adresi'});
  }

}
