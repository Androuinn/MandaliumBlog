import { Component, OnInit} from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BlogService } from '../_services/blog.service';
import { environment } from 'src/environments/environment';



@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  loginModel: any = {};
  date: Date;
  isRegisterClicked = false;
  isHeaderCollapsed = true;
  isCollapsed = true;
  registerForm: FormGroup;
  defaultPhotoUrl = environment.defaultPhotoUrl;

  constructor(
    public authService: AuthService,
    private router: Router,
    private alertify: AlertifyService,
    private formBuilder: FormBuilder,
    private blogService: BlogService
  ) {
    this.registerForm = formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(50) ]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(16) ]],
      confirmpassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(16)]],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      surname: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      birthdate: Date
    });
  }

  ngOnInit() {
    this.date = new Date();
  }

  login() {
    this.authService.login(this.loginModel).subscribe(
      (next) => {
        this.alertify.success('Giriş başarılı');
        this.loginModel = {};
      },
      (error) => {
        this.alertify.error('Kullanıcı adı veya şifre hatalı');
      },
      () => {
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

  changeWriterEntry(writerEntry: boolean) {
    this.blogService.changeBlogIsWriterEntry(writerEntry);
    this.blogService.changeBlogPagination({currentPage: 1, totalPages: 1, totalItems: 1, itemsPerPage: 7});
  }
}
