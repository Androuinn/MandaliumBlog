import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/writer/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;


  constructor(private http: HttpClient) {}




  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
        }
      })
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }


  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  sendActivationPin(Pin: string) {
    const headers = new HttpHeaders({'Content-Type': 'application/json; charset=utf-8'});

    return this.http.post(this.baseUrl + 'confirm', Pin, {headers:headers} );
  }

}
