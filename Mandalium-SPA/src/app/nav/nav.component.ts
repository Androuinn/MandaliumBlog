import { Component, OnInit } from '@angular/core';
import {formatDate} from '@angular/common';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
date: Date;
  constructor(public authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.date = new Date();
  }

  login() {
    this.authService.login(this.model).subscribe(
      next => {
        console.log('Logged in successfully');
      },
      error => {
        console.log(error);
      }, () => {
        this.router.navigate(['/']);
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    console.log('logged out');
    this.router.navigate(['/']);
  }


}
