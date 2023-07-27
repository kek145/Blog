import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { IRegistrationDto } from '../models/registrationDto';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  constructor(private http: HttpClient) { }

  public registration(user: IRegistrationDto): Observable<IRegistrationDto> {
    return this.http.post<IRegistrationDto>(`${environment.apiUrl}/Registration/SignUp`, user);
  }
}
