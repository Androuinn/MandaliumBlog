import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { JwtModule} from '@auth0/angular-jwt';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import localeTr from '@angular/common/locales/tr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { BlogListComponent } from './blog/blog-list/blog-list.component';
import { BlogDetailedComponent } from './blog/blog-detailed/blog-detailed.component';
import { BlogEntryResolver } from './_resolvers/blogEntry.resolver';
import { BlogMostReadComponent } from './blog/blog-most-read/blog-most-read.component';
import { FooterComponent } from './footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AboutComponent } from './about/about.component';
import { CreateBlogEntryComponent } from './user/create-blog-entry/create-blog-entry.component';
import { ContactComponent } from './contact/contact.component';
import { AuthGuard } from './_guards/auth.guard';
import { UsermenuComponent } from './user/usermenu/usermenu.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { BlogEntriesResolver } from './_resolvers/blogEntries.resolver';
import { RegisterComponent } from './user/register/register.component';
import { BlogCommentsComponent } from './blog/blog-comments/blog-comments.component';
import { HttpRequestInterceptor } from './_interceptors/httpRequest-interceptor';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';




registerLocaleData(localeTr, 'tr-TR');

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      BlogListComponent,
      BlogDetailedComponent,
      BlogMostReadComponent,
      FooterComponent,
      AboutComponent,
      CreateBlogEntryComponent,
      ContactComponent,
      UsermenuComponent,
      RegisterComponent,
      BlogCommentsComponent
   ],
   imports: [
      CKEditorModule,
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      CollapseModule.forRoot(),
      PaginationModule.forRoot(),
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      }),
      ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
      TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useFactory: httpTranslateLoader,
          deps:[HttpClient]
        }
      })
   ],
   providers: [
      BlogEntryResolver,
      BlogEntriesResolver,
      AuthGuard,
      {provide: LOCALE_ID, useValue: 'tr-TR'},
      {provide: HTTP_INTERCEPTORS, useClass: HttpRequestInterceptor, multi:true , deps:[TranslateService]}
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }

export function httpTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
