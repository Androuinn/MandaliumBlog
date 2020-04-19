import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';
import { Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  date = new Date();
  constructor(private authService: AuthService, private metaTagService: Meta) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }

    this.metaTagService.addTags([
      // { name: 'decpription', content: 'Mandalium | Blog Dünyasının Vazgeçilmez Adresi, Eğlence ve Haber Sitesi'},
      { name: 'keywords', content: 'Mandalium, Blog Dünyasının Vazgeçilmez Adresi, Eğlence ve Haber Sitesi, '},
      { name: 'robots', content: 'index, follow'},
      { name: 'author', content: 'Tugay Mandal'},
      { name: 'date', content:  this.date.toString() , scheme: 'DD-MM-YYYY'},
      { property: 'og:site_name', content: 'Mandalium'},
      { property: 'og:url', content: 'https://mandalium.azurewebsites.net'},
      { property: 'og:image', content: 'https://res.cloudinary.com/dpwbfco4g/image/upload/v1587061001/%C3%A7zgisiz_logo_ddiqau.png'},
      { property: 'og:title', content: 'Mandalium | En son Haberler'},
      { property: 'og:description', content: 'Mandalium '}
    ]);

    this.metaTagService.updateTag({name: 'description', content: 'Mandalium | Blog Dünyasının Vazgeçilmez Adresi, Eğlence ve Haber Sitesi'});
  }
}
