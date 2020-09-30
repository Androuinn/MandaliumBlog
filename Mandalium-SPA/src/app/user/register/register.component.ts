import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loginModel: any = {};
  activationPin: any;
  registerposted: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private alertify: AlertifyService,
    private authService: AuthService,
    private router: Router
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
  }

  register() {
    const a = this.registerForm.value;
    const password = this.registerForm.get('password').value;
    const confirmPassword = this.registerForm.get('confirmpassword').value;
    const username = this.registerForm.get('username').value;

    if (password !== confirmPassword) {
      this.alertify.error('Girdiğiniz şifreler uyuşmuyor.');
      return;
    }

    this.registerForm.reset();
    return this.authService.register(a).subscribe(() => {
    this.loginModel.password = password;
    this.loginModel.username = username;
    // this.login();
    this.registerposted = true;
    }, error => {
      this.alertify.error('Kullanıcı Mevcut');
    });
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

sendActivationPin() {
  this.authService.sendActivationPin(this.activationPin).subscribe(() => {
    this.alertify.success('Başarılı');
    this.registerposted = false;
    this.router.navigate(['/']);
    this.login();
  },
  error => {
    this.alertify.error(error);
    this.registerposted = false;
  });
}


}
