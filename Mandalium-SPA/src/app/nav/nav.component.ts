import { Component, OnInit } from '@angular/core';
import {formatDate} from '@angular/common';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  date: Date;
  isHeaderCollapsed = true;
  isCollapsed = true;

  constructor(public authService: AuthService, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.date = new Date();
  }

  login() {
    this.authService.login(this.model).subscribe(
      next => {
        this.alertify.success('Giriş başarılı');
      },
      error => {
        this.alertify.error('Kullanıcı adı veya şifre hatalı');
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
    this.alertify.message('Çıkış yapıldı');
    this.router.navigate(['/']);
  }


}
