import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Photo } from '../_models/Photo';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }


  postPhoto(formData: FormData) {
    return this.http.post(this.baseUrl + 'photos', formData);
  }

  getPhotos(page?, itemsPerPage?, userParams?, writerId?): Observable<PaginatedResult<Photo[]>> {
    const paginatedResult: PaginatedResult<Photo[]> = new PaginatedResult<Photo[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('writerEntry', userParams);
    }

    if (writerId >= 1) {
      params = params.append('writerId', writerId);
    }


    return this.http.get<Photo[]>(this.baseUrl + 'photos', { observe: 'response', params, headers: {'Content-Type': 'application/json'}}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }


}
