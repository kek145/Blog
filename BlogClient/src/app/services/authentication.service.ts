import { Observable, tap } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IAuthenticationDto } from '../models/authenticationDto';
import { environment } from 'src/environments/environment';
import { IJwtAuthenticationDto } from '../models/jwtAuthDto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  public authentication(user: IAuthenticationDto) : Observable<IJwtAuthenticationDto> {
    const options = {
      withCredentials: true
    };
    return this.http.post<IJwtAuthenticationDto>(`${environment.apiUrl}/Authentication/SignIn`, user, options);
  }
}
