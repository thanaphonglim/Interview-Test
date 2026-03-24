import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserModel } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {

  private apiUrl = `${environment.apiUrl}/User`;
  private getHeaders() {
    return new HttpHeaders({
      'x-api-key': 'EXIMAPIKEY'
    });
  }
  constructor(private http: HttpClient) { }

  getUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.apiUrl, {
      headers: this.getHeaders()
    });
  }

  getUserById(id: string | null): Observable<UserModel> {
    return this.http.get<UserModel>(`${this.apiUrl}/GetUserById/${id}`, {
      headers: this.getHeaders()
    });
  }

}
