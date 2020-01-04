import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BlogDetailedComponent } from './blog/blog-list/blog-detailed/blog-detailed.component';
import { BlogEntryResolver } from './_resolvers/blogEntry.resolver';

import { BlogListComponent } from './blog/blog-list/blog-list.component';
import { AboutComponent } from './about/about.component';
import { CreateBlogEntryComponent } from './blog/blog-list/create-blog-entry/create-blog-entry.component';
import { BlogWriterEntryComponent } from './blog/blog-list/blog-writerEntry/blog-writerEntry.component';
import { ContactComponent } from './contact/contact.component';
import { AuthGuard } from './_guards/auth.guard';



const routes: Routes = [
  {path: '', component: BlogListComponent},
  {path: 'personalblog', component: BlogWriterEntryComponent},
  {path: 'about', component: AboutComponent},
  {path: 'contact', component: ContactComponent},
  {path: 'blog/:id/:headline', component: BlogDetailedComponent, resolve: {blog: BlogEntryResolver}},
  {path: 'create', component: CreateBlogEntryComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
  {path: '**', redirectTo: 'home', pathMatch: 'full'}
 ];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
