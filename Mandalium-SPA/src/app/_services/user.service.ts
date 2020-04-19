import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/Writer';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl + 'user';
  constructor(private http: HttpClient) {}
  getUser(): Observable<User> {
    return this.http.get<User>(this.baseUrl + '/getuser');
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/getusers');
  }

  updateUser(user: User) {
    return this.http.put(this.baseUrl + '/updateuser', user);
  }
}
