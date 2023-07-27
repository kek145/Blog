import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { IRegistrationDto } from 'src/app/models/registrationDto';
import { RegistrationService } from 'src/app/services/registration.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  selectedValueUser: string = 'User';
  selectedValueAuthor: string = 'Author';
  registrationDto: IRegistrationDto = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    role: ''
  };

  constructor(private registrationService: RegistrationService, private router: Router) {}

  registrationUser(user: IRegistrationDto): void {
    this.registrationService.registration(user).subscribe(
      response => {
        Swal.fire('Successfully', 'Account created successfully!', 'success');
        this.router.navigate(['/login']);
      },
      error => {
        Swal.fire('Error', `${error.error}`, 'error');
      }
    );
  }
}
