import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BlogDetailedComponent } from './blog/blog-detailed/blog-detailed.component';
import { BlogEntryResolver } from './_resolvers/blogEntry.resolver';

import { AboutComponent } from './about/about.component';
import { CreateBlogEntryComponent } from './user/create-blog-entry/create-blog-entry.component';
import { ContactComponent } from './contact/contact.component';
import { AuthGuard } from './_guards/auth.guard';
import { UsermenuComponent } from './user/usermenu/usermenu.component';
import { BlogEntriesResolver } from './_resolvers/blogEntries.resolver';
import { RegisterComponent } from './user/register/register.component';



const routes: Routes = [
  {path: 'home', component: HomeComponent , resolve: {entries: BlogEntriesResolver}, runGuardsAndResolvers: 'always' },
  {path: 'register', component: RegisterComponent},
  // {path: 'blog', component: BlogListComponent},
  // {path: 'personalblog', component: HomeComponent , resolve: {entries: BlogEntriesResolver}},
  {path: 'about', component: AboutComponent},
  {path: 'contact', component: ContactComponent},
  {path: 'blog/:id/:headline', component: BlogDetailedComponent, resolve: {blog: BlogEntryResolver}},
  {path: 'profile', component: UsermenuComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
  {path: 'create', component: CreateBlogEntryComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
  {path: '**', redirectTo: 'home', pathMatch: 'full'}
 ];



@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled',
                            anchorScrolling: 'enabled',
                             scrollOffset: [0, 64], preloadingStrategy: PreloadAllModules
                             })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
