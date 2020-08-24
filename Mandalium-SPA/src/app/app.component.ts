import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';
import { Meta } from '@angular/platform-browser';
import { Router, RouterEvent, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  date = new Date();
  public showOverlay = true;
  constructor(private authService: AuthService, private metaTagService: Meta, private router: Router) {
    router.events.subscribe((event: RouterEvent) => {
      this.navigationInterceptor(event);
    });
  }

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

    this.metaTagService.updateTag({name: 'description',
    content: 'Mandalium | Blog Dünyasının Vazgeçilmez Adresi, Eğlence ve Haber Sitesi'});
  }

  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.showOverlay = true;
    }
    if (event instanceof NavigationEnd) {
      this.showOverlay = false;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.showOverlay = false;
    }
    if (event instanceof NavigationError) {
      this.showOverlay = false;
    }
  }
}
