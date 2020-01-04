import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule} from '@angular/common/http';
import { PaginationModule, BsDropdownModule } from 'ngx-bootstrap';
import { JwtModule} from '@auth0/angular-jwt';

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
import { CreateBlogEntryComponent } from './blog/blog-list/create-blog-entry/create-blog-entry.component';
import { BlogWriterEntryComponent } from './blog/blog-list/blog-writerEntry/blog-writerEntry.component';
import { ContactComponent } from './contact/contact.component';
import { AuthGuard } from './_guards/auth.guard';


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
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      PaginationModule.forRoot(),
      BsDropdownModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      BlogEntryResolver,
      AuthGuard
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
