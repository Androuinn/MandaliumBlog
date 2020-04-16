import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Writer } from '../_models/Writer';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}
  getWriter(): Observable<Writer> {
    return this.http.get<Writer>(this.baseUrl + 'user/getwriter');
  }

  getWriters(): Observable<Writer[]> {
    return this.http.get<Writer[]>(this.baseUrl + 'user/getwriters');
  }

  updateWriter(writer: Writer) {
    return this.http.put(this.baseUrl + 'user/updatewriter', writer);
  }
}
