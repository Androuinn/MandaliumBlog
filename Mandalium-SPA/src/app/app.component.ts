import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';
import { Router, RouterEvent, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { MetaService } from './_services/meta.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  date = new Date();
  public showOverlay = true;
  constructor(private authService: AuthService, private router: Router, private metaService: MetaService, public translate: TranslateService) {
    router.events.subscribe((event: RouterEvent) => {
      this.navigationInterceptor(event);
    });
    translate.addLangs(['tr-TR', 'en-US']);
    var lang = localStorage.getItem("lang");

    if (lang != null){
      translate.setDefaultLang(lang);
    } else{
      translate.setDefaultLang('tr-TR');
      localStorage.setItem("lang","tr-TR");
    }
  }

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    this.metaService.CreateSiteTags();
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

  switchLang(lang: string) {
    this.translate.use(lang);
    localStorage.setItem("lang",lang);
  }
}
