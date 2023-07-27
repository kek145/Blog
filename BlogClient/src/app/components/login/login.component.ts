import Swal from 'sweetalert2';
import { Component } from '@angular/core';
import { IAuthenticationDto } from 'src/app/models/authenticationDto';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { IJwtAuthenticationDto } from 'src/app/models/jwtAuthDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  jwtTokenDto: IJwtAuthenticationDto = {
    token: ''
  };
  authenticationDto: IAuthenticationDto = {
    email: '',
    password: ''
  };
  constructor(private authenticationService: AuthenticationService) { }

  authentiocation(user: IAuthenticationDto) : void {
    this.authenticationService.authentication(user).subscribe(
      response => {
        Swal.fire('Successfully', 'Authentication successful!', 'success');
      },
      error => {
        Swal.fire('Error', 'Wrong email or password!', 'error');
      }
    );
  }
}
