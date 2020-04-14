import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClientModule} from '@angular/common/http';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { JwtModule} from '@auth0/angular-jwt';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import localeTr from '@angular/common/locales/tr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { BlogListComponent } from './blog/blog-list/blog-list.component';
import { BlogDetailedComponent } from './blog/blog-list/blog-detailed/blog-detailed.component';
import { BlogEntryResolver } from './_resolvers/blogEntry.resolver';
import { BlogMostReadComponent } from './blog/blog-list/blog-most-read/blog-most-read.component';
import { FooterComponent } from './footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AboutComponent } from './about/about.component';
import { CreateBlogEntryComponent } from './user/create-blog-entry/create-blog-entry.component';
import { BlogWriterEntryComponent } from './blog/blog-list/blog-writerEntry/blog-writerEntry.component';
import { ContactComponent } from './contact/contact.component';
import { AuthGuard } from './_guards/auth.guard';
import { UsermenuComponent } from './user/usermenu/usermenu.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';




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
      BlogWriterEntryComponent,
      ContactComponent,
      UsermenuComponent
   ],
   imports: [
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
      ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
   ],
   providers: [
      BlogEntryResolver,
      AuthGuard,
      {provide: LOCALE_ID, useValue: 'tr-TR'}
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
